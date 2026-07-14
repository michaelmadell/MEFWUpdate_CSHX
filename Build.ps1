#Requires -Version 5.1
<#
.SYNOPSIS
    Build MEFWUpdate Release (Any CPU) using dotnet CLI.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$sln = Join-Path $PSScriptRoot "MEFWUpdate.sln"

Write-Host "Config  : Release | Any CPU" -ForegroundColor DarkGray
Write-Host ""

dotnet build $sln -c Release --nologo

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "BUILD FAILED  (exit $LASTEXITCODE)" -ForegroundColor Red
    exit $LASTEXITCODE
}

$exe = Join-Path $PSScriptRoot "MEFWUpdate\bin\Release\net48\MEFWUpdate.exe"
if (Test-Path $exe) {
    $size = "{0:N0} KB" -f (([IO.FileInfo]$exe).Length / 1KB)
    Write-Host ""
    Write-Host "BUILD OK  ->  $exe  ($size)" -ForegroundColor Green
} else {
    Write-Host "BUILD OK but output exe not found at expected path." -ForegroundColor Yellow
}
