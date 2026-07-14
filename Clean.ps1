#Requires -Version 5.1
<#
.SYNOPSIS
    Remove all build artifacts for MEFWUpdate.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$sln = Join-Path $PSScriptRoot "MEFWUpdate.sln"

dotnet clean $sln -c Release --nologo 2>$null

# Remove bin/obj and dist (.vs is VS workspace cache - skip, may be locked by VS)
$targets = @(
    "MEFWUpdate\bin"
    "MEFWUpdate\obj"
    "dist"
)

foreach ($rel in $targets) {
    $path = Join-Path $PSScriptRoot $rel
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force
        Write-Host "Removed  $rel" -ForegroundColor DarkGray
    }
}

Write-Host ""
Write-Host "Clean complete." -ForegroundColor Green
