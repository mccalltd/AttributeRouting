param(
    $version,
    $tasks = "Default"
)


#========================================
# Ensure we have required params
#========================================

while (-not $version) { $version = Read-Host "Version" }
while (-not $tasks) { $tasks = Read-Host "Tasks" }


#========================================
# Run the build
#========================================

Import-Module .\build\tools\psake\psake.psm1 -Force
Invoke-psake .\build\default.ps1 $tasks `
    -parameters @{
        "v" = "$version"
    }

if (-not $psake.build_success) {
    Write-Host "Error during build!" -ForegroundColor Red
}