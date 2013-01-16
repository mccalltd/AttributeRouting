param(
    $tasks = "Default",
    $version
)


#========================================
# Ensure we have required params
#========================================

while (-not $tasks) { $tasks = Read-Host "Tasks" }
while (-not $version) { $version = Read-Host "Version" }


#========================================
# Run the build
#========================================

$base_dir = Split-Path $MyInvocation.MyCommand.Definition
Import-Module $base_dir\tools\psake\psake.psm1 -Force
Invoke-psake $base_dir\default.ps1 $tasks -properties @{ "version" = $version }

if (-not $psake.build_success) {
    Write-Host "Error during build!" -ForegroundColor Red
}