#Requires -Version 5.1
<#
.SYNOPSIS
    Clean-build MEFWUpdate and copy the final exe to .\dist\.

.DESCRIPTION
    Runs a full clean then rebuild so the embedded FWUpdLcl64.exe and
    fw_update.bin are always freshly baked into the output executable.
    The result is a single self-contained .exe in .\dist\.

.PARAMETER SkipClean
    Skip the clean step (faster, but stale resource embeds may persist).
#>

param(
    [switch]$SkipClean
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$root    = $PSScriptRoot
$distDir = Join-Path $root "dist"
$srcExe  = Join-Path $root "MEFWUpdate\bin\Release\net48\MEFWUpdate.exe"
$dstExe  = Join-Path $distDir "CoreStation_HX3000_Intel_ME_FW_Update_20.26.1.1.exe"

# -- 1. Verify firmware assets are present -----------------------------------
$tool = Join-Path $root "MEFWUpdate\FWUpdLcl64.exe"
$fw   = Join-Path $root "MEFWUpdate\fw_update.bin"

$missing = @()
if (-not (Test-Path $tool)) { $missing += "MEFWUpdate\FWUpdLcl64.exe" }
if (-not (Test-Path $fw))   { $missing += "MEFWUpdate\fw_update.bin"  }

if ($missing.Count -gt 0) {
    Write-Host ""
    Write-Host "ERROR: Required firmware assets not found:" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "       $_" -ForegroundColor Red }
    Write-Host ""
    Write-Host "Place FWUpdLcl64.exe and fw_update.bin in MEFWUpdate\ before publishing." -ForegroundColor Yellow
    exit 1
}

Write-Host "Assets  : FWUpdLcl64.exe + fw_update.bin found" -ForegroundColor DarkGray

# -- 2. Clean ----------------------------------------------------------------
if (-not $SkipClean) {
    Write-Host "Cleaning previous build..." -ForegroundColor DarkGray
    & "$root\Clean.ps1"
    Write-Host ""
}

# -- 3. Build ----------------------------------------------------------------
Write-Host "Building Release..." -ForegroundColor Cyan
& "$root\Build.ps1"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "PUBLISH ABORTED - build failed." -ForegroundColor Red
    exit $LASTEXITCODE
}

# -- 4. Copy to dist\ -------------------------------------------------------
if (-not (Test-Path $srcExe)) {
    Write-Host "PUBLISH ABORTED - exe not found at: $srcExe" -ForegroundColor Red
    exit 1
}

New-Item -ItemType Directory -Force -Path $distDir | Out-Null
Copy-Item $srcExe $dstExe -Force

$info = [IO.FileInfo]$dstExe
$size = "{0:N0} KB" -f ($info.Length / 1KB)
$hash = (Get-FileHash $dstExe -Algorithm SHA256).Hash

Write-Host ""
Write-Host "----------------------------------------------" -ForegroundColor DarkGray
Write-Host "PUBLISHED" -ForegroundColor Green
Write-Host "  Path  : $dstExe" -ForegroundColor White
Write-Host "  Size  : $size" -ForegroundColor White
Write-Host "  SHA256: $hash" -ForegroundColor DarkGray
Write-Host "----------------------------------------------" -ForegroundColor DarkGray
