#========================================
# Set up
#========================================

Properties {
    $base_dir = Resolve-path .
    $source_dir = "$base_dir\src"
    $tools_dir = "$base_dir\tools"
    $nuspec_dir = "$base_dir\nuget"
    $build_dir = "$base_dir\build"
    $bin_dir = "$build_dir\bin"
    $nupkg_dir = "$build_dir\nuget"
    $version = "0.0"
    $now = Get-Date
    $nuget = "$source_dir\.nuget\nuget.exe"
}

FormatTaskName (("-" * 25) + " [{0}] " + ("-" * 25))


#========================================
# Tasks
#========================================

Task Default -depends Build
Task Build -depends Clean, CreateSharedAssemblyInfo, Rebuild, Test
Task Package -depends Build, NugetPack
Task Publish -depends Package, NugetPush

Task Clean {
    Clean-Directory $build_dir
}

Task CreateSharedAssemblyInfo {
    $file_name = "$source_dir\SharedAssemblyInfo.cs"
    Create-SharedAssemblyInfo $file_name $version
}

Task Rebuild {
    $solution_file = "$source_dir\AttributeRouting.sln"
    Write-Host "Building $solution_file" -ForegroundColor Green
    Exec { msbuild $solution_file /t:Rebuild /p:Configuration=Release /p:OutDir=$bin_dir /v:minimal /nologo } 
}

Task Test {
    $nunit = "$tools_dir\nunit\nunit-console-x86.exe"
    $test_assemblies = "$bin_dir\AttributeRouting.Specs.dll"
    Write-Host "Running tests in $test_assemblies" -ForegroundColor Green
    Exec { &$nunit $test_assemblies /work:$build_dir /out:TestResults.txt /result:TestResults.xml /nologo /nodots }
}

Task NugetPack {
    Clean-Directory $nupkg_dir
    Get-ChildItem "$nuspec_dir\*" -Include "*.nutrans" -Recurse | foreach { Create-Nupkg $_ } 
}

Task NugetPush {
    Get-ChildItem "$nupkg_dir\*" -Include "*.nupkg" -Exclude "*.symbols.nupkg" | foreach { Push-Nupkg $_ }
}


#========================================
# Helper functions
#========================================

function Clean-Directory ($dir) {
    Write-Host "Cleaning $dir" -ForegroundColor Green
    if (Test-Path $dir) { Remove-Item $dir -Force -Recurse | Out-Null }
    New-Item $dir -ItemType Directory | Out-Null
}

function Create-Nupkg ($nutrans_path) {
	$name = [System.IO.Path]::GetFileNameWithoutExtension($nutrans_path)
    $nuspec = Create-Nuspec $name
    Write-Host "Creating nupkg for $name" -ForegroundColor Green
    Exec { &$nuget pack $nuspec -Version $version -OutputDirectory $nupkg_dir }
}

function Create-Nuspec ($name) {
    $transform_xml = "$tools_dir\TransformXml.proj"
    $shared_nuspec = "$nuspec_dir\AttributeRouting.Shared.nuspec"
    $nutrans = "$nuspec_dir\$name\$name.nutrans"
    $nuspec = "$nuspec_dir\$name\$name.nuspec"
    Write-Host "Creating nuspec for $name" -ForegroundColor Green
    Exec { msbuild $transform_xml /p:Source=$shared_nuspec /p:Transform=$nutrans /p:Destination=$nuspec /v:minimal /nologo } 
    $nuspec
}

function Push-Nupkg ($nupkg) {
    Write-Host "Pushing $nupkg to Nuget gallery" -ForegroundColor Green
    Exec { &$nuget push $nupkg }
}

function Create-SharedAssemblyInfo ($file, $version) {
    $non_prerelease_version = ($version -split "-")[0]
    Write-Host "Creating $file for $version" -ForegroundColor Green
    @"
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AttributeRouting")]
[assembly: AssemblyCopyright("Copyright 2010-$($now.Year) Tim McCall")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyVersion("$non_prerelease_version")]
[assembly: AssemblyFileVersion("$non_prerelease_version")]
[assembly: AssemblyInformationalVersion("$version")]
[assembly: AssemblyConfiguration("Release")]
"@ | Out-File $file_name -Encoding ascii
}