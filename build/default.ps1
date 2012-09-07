#========================================
# Set up
#========================================

Properties {
    $base_dir = Resolve-path ..
    $build_dir = "$base_dir\build"
    $tools_dir = "$build_dir\tools"
    $bin_dir = "$build_dir\bin"
    $out_dir = "$build_dir\out"
    $nupkg_dir = "$build_dir\nuget"
    $nuspec_dir = "$base_dir\nuget"
    $source_dir = "$base_dir\src"
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
Task Publish -depends Build, NugetPack, NugetPush

Task Clean {
    Clean-Directory $bin_dir
    Clean-Directory $out_dir
}

Task CreateSharedAssemblyInfo {
    $file_name = "$source_dir\SharedAssemblyInfo.cs"
    $non_prerelease_version = ($version -split "-")[0]
    
    Write-Host "Creating $file_name for $version" -ForegroundColor Green
    
    "using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyCompany("""")]
[assembly: AssemblyProduct(""AttributeRouting"")]
[assembly: AssemblyCopyright(""Copyright Tim McCall 2010-" + $now.Year + """)]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyVersion(""$non_prerelease_version"")]
[assembly: AssemblyFileVersion(""$non_prerelease_version"")]
[assembly: AssemblyInformationalVersion(""$version"")]
[assembly: AssemblyConfiguration(""Release"")]" | 
    Out-File $file_name -Encoding ascii
}

Task Rebuild {
    $solution_file = "$source_dir\AttributeRouting.sln"
    Write-Host "Building $solution_file" -ForegroundColor Green
    Exec { msbuild $solution_file /t:Rebuild /p:Configuration=Release /p:OutDir=$bin_dir /v:minimal /nologo } 
}

Task Test {
    $nunit = "$tools_dir\NUnit\nunit-console-x86.exe"
    $test_assemblies = "$bin_dir\AttributeRouting.Specs.dll"
    Write-Host "Running tests in $test_assemblies" -ForegroundColor Green
    Exec { &$nunit $test_assemblies /work:$out_dir /out:TestResults.txt /result:TestResults.xml /nologo /nodots }
}

Task NugetPack {
    Clean-Directory $nupkg_dir
    Get-ChildItem $nuspec_dir -Directory | foreach { Create-Nupkg $_ } 
}

Task NugetPush {
    Get-ChildItem $nupkg_dir | foreach { 
		$file_name = $_.Name
		$path = (Join-Path $_.Directory $file_name)
		Write-Host "Pushing $file_name to nuget server" -ForegroundColor Green
	} 
        #| Exec { &$nuget push (Join-Path $_.Directory $_.Name) }
}


#========================================
# Helper functions
#========================================

function Clean-Directory ($dir) {
    Write-Host "Cleaning $dir" -ForegroundColor Green
    if (Test-Path $dir) { Remove-Item $dir -Force -Recurse | Out-Null }
    New-Item $dir -ItemType Directory | Out-Null
}

function Create-Nupkg ($name) {
    $nuspec = Create-Nuspec $name
    Write-Host "Creating nupkg for $name" -ForegroundColor Green
    Exec { &$nuget pack $nuspec -Version $version -OutputDirectory $nupkg_dir }
}

function Create-Nuspec ($name) {
    $nuspec = "$nuspec_dir\$name\$name.nuspec"
    Write-Host "Creating nuspec for $name" -ForegroundColor Green
    Exec { 
        msbuild .\TransformXml.proj /v:minimal /nologo `
            /p:Source=$nuspec_dir\AttributeRouting.Shared.nuspec `
            /p:Transform=$nuspec_dir\$name\$name.nutrans `
            /p:Destination=$nuspec 
    } 
    $nuspec
}

