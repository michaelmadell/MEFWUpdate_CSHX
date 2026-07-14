# Intel ME Firmware Update — CoreStation HX3000

Update package for the Intel Converged Security Management Engine (CSME) firmware on CoreStation HX3000 nodes.

| | |
|---|---|
| **ME Firmware version** | 19.0.11.2289 |
| **Package version** | 20.26.1.1 |
| **Platform** | CoreStation HX3000 |
| **Publisher** | Amulet Hotkey Ltd |

---

## Overview

A self-contained Windows executable that embeds the Intel firmware update tool (`FWUpdLcl64.exe`) and the firmware image (`fw_update.bin`). On launch it:

1. Requests UAC elevation (administrator required)
2. Displays a consent screen with safety warnings
3. Extracts embedded assets to a temporary directory
4. Runs the update tool and streams all output to an on-screen console
5. Blocks system shutdown and prevents the window being closed for the duration of the update
6. Enforces a 15-minute safety timeout after which close is re-enabled

## Requirements

- Windows 10 / 11 (x64)
- .NET Framework 4.8 (pre-installed on Windows 10 1903+ and Windows 11)
- Administrator rights
- AC power connected before running

## Usage

Run `CoreStation_HX3000_Intel_ME_FW_Update_<version>.exe` and follow the on-screen instructions. Do not power off or restart the system until the update reports success.

---

## Building from source

### Prerequisites

- [.NET SDK 6+](https://dotnet.microsoft.com/download) (for `dotnet build`)
- `FWUpdLcl64.exe` and `fw_update.bin` placed in `MEFWUpdate\`

### Scripts

| Script | Purpose |
|---|---|
| `.\Build.ps1` | Build Release configuration |
| `.\Clean.ps1` | Remove all build artifacts |
| `.\Publish.ps1` | Clean build + copy final exe to `dist\` |

```powershell
# Full publish
.\Publish.ps1

# Incremental build (skip clean)
.\Publish.ps1 -SkipClean
```

Output: `dist\CoreStation_HX3000_Intel_ME_FW_Update_<version>.exe`

### Firmware assets

The firmware binaries are **not committed** to source control. Obtain them from the Intel CSME release package and place them alongside the project file before building:

```
MEFWUpdate\
  FWUpdLcl64.exe   <- Intel update tool
  fw_update.bin    <- CSME firmware image
```

---

## Project structure

```
MEFWUpdate\
  MEFWUpdate.sln
  Build.ps1
  Clean.ps1
  Publish.ps1
  MEFWUpdate\
    MEFWUpdate.csproj
    app.manifest
    Program.cs
    MainForm.cs
    MainForm.Designer.cs
    MainForm.resx
```

---

## Safety notes

- **Do not interrupt a firmware update in progress.** Doing so may corrupt the ME firmware and require depot-level repair.
- The application blocks `Alt+F4`, the title-bar close button, and Windows shutdown/logoff for the duration of the update.
- The 15-minute timeout is a last-resort safety valve only — if it expires before the update completes, close at your own risk.
