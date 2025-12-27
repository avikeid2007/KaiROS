# MSI Build Script Fixes - WiX v3.14 Compatibility

## Issues Fixed

### 1. ? WiX Toolset Version Mismatch
**Problem**: Project file was configured for WiX v3.11, but you have v3.14 installed.

**Fix Applied**:
- Updated `KAIROS.Installer.wixproj`:
  - Changed `<ProductVersion>` from `3.11` to `3.14`
  - Added explicit WiX tool path detection for both v3.11 and v3.14
  - Added fallback to legacy MSBuild location for WiX targets
  - Updated to find `wix.targets` in: `C:\Program Files (x86)\MSBuild\Microsoft\WiX\v3.x\`

### 2. ? MSBuild Not in PATH
**Problem**: `msbuild` command not found because it's not in system PATH.

**Fix Applied**:
- Added automatic MSBuild detection using:
  - `vswhere.exe` (Visual Studio 2017+)
  - Fallback to common Visual Studio installation paths
  - Now finds MSBuild at: `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe`

### 3. ? .NET Restore Issue with Platform-Specific Builds
**Problem**: MSBuild couldn't find assets file for platform-specific build.

**Fix Applied**:
- Switched from separate `dotnet restore` + `msbuild` to unified `dotnet build`
- `dotnet build` handles restore automatically with correct runtime identifier
- Simplified build process and removed potential restore/build mismatch

## Files Modified

| File | Changes |
|------|---------|
| `KAIROS.Installer.wixproj` | Updated WiX version, added path detection, fixed targets path |
| `Build-MSI.ps1` | Added MSBuild auto-detection, simplified build process |

## Verification

Build script now successfully:
1. ? Locates MSBuild automatically
2. ? Finds WiX Toolset v3.14
3. ? Cleans previous builds
4. ? Builds KAIROS application with correct runtime
5. ? Builds MSI installer
6. ? Generates validation report

## How to Use

```powershell
cd F:\source\KaiROS\KAIROS.Installer
.\Build-MSI.ps1 -Platform x64 -Configuration Release
```

## Expected Output

```
F:\source\KaiROS\KAIROS.Installer\bin\x64\Release\
??? KaiROS-AI-Setup.msi
??? ValidationReport.txt
```

## Compatibility

Now supports:
- ? WiX Toolset v3.11
- ? WiX Toolset v3.14
- ? Visual Studio 2019
- ? Visual Studio 2022
- ? MSBuild Tools (standalone)

## Notes

- WiX v3.14 is fully compatible with the installer definition
- No changes needed to `Product.wxs` file
- GitHub Actions workflow will also work with v3.14
