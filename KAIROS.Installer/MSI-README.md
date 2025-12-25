# MSI Installer Configuration

This project includes a complete MSI installer setup for Microsoft Store submission.

## ?? Quick Start

### Build the MSI

```powershell
cd KAIROS.Installer
.\Build-MSI.ps1 -Platform x64 -Configuration Release
```

### Validate the MSI

```powershell
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
```

### Install the MSI

```powershell
# Interactive
msiexec /i bin\x64\Release\KaiROS-AI-Setup.msi

# Silent
msiexec /i bin\x64\Release\KaiROS-AI-Setup.msi /quiet /qn
```

## ?? Documentation

- **[KAIROS.Installer/README.md](KAIROS.Installer/README.md)** - Installation and building instructions
- **[KAIROS.Installer/SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md)** - Complete Microsoft Store submission guide
- **[KAIROS.Installer/QUICK-REFERENCE.md](KAIROS.Installer/QUICK-REFERENCE.md)** - Command reference

## ? Features

- ? **Silent Installation** - Supports `/quiet` and `/qn` switches
- ? **Microsoft Store Ready** - Meets all certification requirements
- ? **Multiple Platforms** - x64 and x86 support
- ? **Automatic Upgrades** - Seamless version upgrades
- ? **Clean Uninstall** - Complete removal of all files
- ? **Desktop & Start Menu Shortcuts** - Easy access to the app
- ? **Code Signing Support** - Optional digital signing

## ?? Microsoft Store Requirements Met

| Requirement | Status |
|------------|--------|
| Silent installation support | ? |
| Silent uninstallation support | ? |
| Windows 10 1809+ compatibility | ? |
| Per-machine installation | ? |
| Proper upgrade handling | ? |
| Clean registry/file removal | ? |

## ?? Prerequisites

1. **WiX Toolset v3.11+**
   - Download: https://wixtoolset.org/releases/

2. **.NET 8 SDK**
   - Included in Visual Studio 2022

3. **Windows SDK** (for code signing)
   - Optional, for signtool.exe

## ??? Project Structure

```
KAIROS.Installer/
??? Product.wxs              # Main WiX installer definition
??? KAIROS.Installer.wixproj # WiX project file
??? Build-MSI.ps1            # Automated build script
??? Validate-MSI.ps1         # Validation script
??? README.md                # Installation guide
??? SUBMISSION-GUIDE.md      # Store submission guide
??? QUICK-REFERENCE.md       # Command reference
```

## ?? Build Pipeline

The project includes a GitHub Actions workflow that automatically:

1. Builds MSI packages for x64 and x86
2. Runs validation tests
3. Signs packages (if certificate is configured)
4. Creates GitHub releases
5. Prepares Microsoft Store submission packages

### Trigger the workflow:

```bash
# Create and push a version tag
git tag v1.0.2
git push origin v1.0.2
```

## ?? Version Updates

When releasing a new version:

1. Update version in `KAIROS.Installer/Product.wxs`:
   ```xml
   <?define ProductVersion = "1.0.3.0" ?>
   ```

2. Update version in `Package.appxmanifest`:
   ```xml
   <Identity Version="1.0.3.0" />
   ```

3. Rebuild and test:
   ```powershell
   .\Build-MSI.ps1 -Platform x64 -Configuration Release
   .\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
   ```

## ?? Testing

### Automated Testing

```powershell
# Run all validation tests
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
```

### Manual Testing

1. **Silent Install Test**
   ```powershell
   msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log
   ```

2. **Launch Application**
   ```powershell
   & "C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe"
   ```

3. **Silent Uninstall Test**
   ```powershell
   msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log
   ```

### WACK Testing

```powershell
# Run Windows App Certification Kit
& "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcert.exe"
```

## ?? Microsoft Store Submission

### Step 1: Build and Validate

```powershell
# Build release MSI
.\Build-MSI.ps1 -Platform x64 -Configuration Release

# Validate
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
```

### Step 2: Test Thoroughly

- [ ] Test on clean Windows 10 1809+ machine
- [ ] Run Windows App Certification Kit (WACK)
- [ ] Verify all features work
- [ ] Test upgrade from previous version

### Step 3: Submit to Partner Center

1. Go to [Microsoft Partner Center](https://partner.microsoft.com/)
2. Navigate to **Apps and games** > **KaiROS AI**
3. Create new submission
4. Upload `KaiROS-AI-Setup.msi`
5. Complete store listing
6. Submit for certification

See [SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md) for detailed instructions.

## ?? Code Signing

### Sign the MSI

```powershell
# Using Build script
.\Build-MSI.ps1 -Platform x64 -Configuration Release -Sign -CertificatePath "cert.pfx"

# Manual signing
signtool sign /f cert.pfx /p password /t http://timestamp.digicert.com KaiROS-AI-Setup.msi
```

### Verify Signature

```powershell
Get-AuthenticodeSignature KaiROS-AI-Setup.msi
```

## ?? Troubleshooting

### Build Errors

**WiX Toolset not found**
- Install from https://wixtoolset.org/releases/
- Ensure WiX bin folder is in PATH

**KAIROS.exe not found**
- Build KAIROS project first
- Check platform matches (x64 vs x86)

### Installation Errors

**Error 1603 (Fatal error)**
- Run as administrator
- Check install.log for details
- Verify no other version installed

**Application won't launch**
- Check Windows Event Viewer
- Verify all DLLs present
- Test on clean machine

### Certification Failures

See detailed troubleshooting in [SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md#troubleshooting)

## ?? Support

- **GitHub Issues**: https://github.com/avikeid2007/KaiROS/issues
- **Email**: avikeid2007@gmail.com
- **Documentation**: See KAIROS.Installer/SUBMISSION-GUIDE.md

## ?? License

Same as parent project - see [LICENSE](../LICENSE)

## ?? Acknowledgments

- [WiX Toolset](https://wixtoolset.org/) - Windows Installer XML toolkit
- [Microsoft Store Certification](https://learn.microsoft.com/en-us/windows/apps/publish/) - Guidelines and documentation
