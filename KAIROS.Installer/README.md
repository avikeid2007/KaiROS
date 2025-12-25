# KaiROS AI - MSI Installer for Microsoft Store

This directory contains the WiX Toolset installer project for creating an MSI package that can be submitted to the Microsoft Store.

## Prerequisites

1. **WiX Toolset v3.11 or later**
   - Download from: https://wixtoolset.org/releases/
   - Install the WiX Toolset build tools

2. **Visual Studio WiX Extension** (Optional, for IDE integration)
   - Install the "WiX Toolset Visual Studio Extension" from the Visual Studio Marketplace

## Building the MSI

### Using Visual Studio
1. Open the KAIROS solution
2. Build the KAIROS project first (Release configuration)
3. Build the KAIROS.Installer project
4. The MSI will be in `KAIROS.Installer\bin\{Platform}\Release\KaiROS-AI-Setup.msi`

### Using Command Line

```powershell
# Build for x64
dotnet build KAIROS.csproj -c Release -p:Platform=x64
msbuild KAIROS.Installer\KAIROS.Installer.wixproj /p:Configuration=Release /p:Platform=x64

# Build for x86
dotnet build KAIROS.csproj -c Release -p:Platform=x86
msbuild KAIROS.Installer\KAIROS.Installer.wixproj /p:Configuration=Release /p:Platform=x86
```

## Silent Installation (Required for Microsoft Store)

The MSI supports silent installation which is required for Microsoft Store submission:

```cmd
# Silent install
msiexec /i KaiROS-AI-Setup.msi /quiet /qn

# Silent install with log
msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log

# Silent uninstall
msiexec /x KaiROS-AI-Setup.msi /quiet /qn
```

## Microsoft Store Submission Requirements

This MSI package is configured to meet Microsoft Store requirements:

### ? Certification Requirements Met

1. **Silent Installation Support**
   - Supports `/quiet` and `/qn` switches
   - No user interaction required during silent install

2. **Proper Versioning**
   - Product version: 1.0.2.0
   - Upgrade code ensures proper upgrade handling
   - Major upgrades configured correctly

3. **Windows Version Requirements**
   - Minimum: Windows 10 1809 (Build 17763)
   - Matches the app's minimum requirements

4. **Installation Scope**
   - Per-machine installation (`InstallScope="perMachine"`)
   - Requires administrator privileges

5. **No Registry Pollution**
   - Only necessary registry entries for app functionality
   - Clean uninstallation

6. **Proper Component GUIDs**
   - All components have stable GUIDs
   - Ensures proper updates and servicing

### ?? Pre-Submission Checklist

Before submitting to Microsoft Store:

- [ ] Test silent installation: `msiexec /i KaiROS-AI-Setup.msi /quiet /qn`
- [ ] Test silent uninstallation: `msiexec /x KaiROS-AI-Setup.msi /quiet /qn`
- [ ] Test upgrade from previous version
- [ ] Verify all DLLs are included in the MSI
- [ ] Verify desktop shortcut creation (optional)
- [ ] Verify Start Menu shortcuts
- [ ] Test on clean Windows 10 1809+ machine
- [ ] Run Windows App Certification Kit (WACK)
- [ ] Verify code signing (if applicable)

### ?? Manual Package Validation

As per https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/manual-package-validation:

1. **Install Validation**
   ```cmd
   msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log
   ```
   - Verify exit code is 0 (success)
   - Check install.log for errors
   - Verify application launches correctly

2. **Uninstall Validation**
   ```cmd
   msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log
   ```
   - Verify exit code is 0 (success)
   - Check uninstall.log for errors
   - Verify all files and folders are removed

3. **Upgrade Validation**
   - Install version 1.0.1.0
   - Install version 1.0.2.0 (current)
   - Verify upgrade completes successfully
   - Verify application works after upgrade

4. **File Verification**
   - Extract MSI using: `msiexec /a KaiROS-AI-Setup.msi /qn TARGETDIR=C:\Temp\Extract`
   - Verify all required files are present
   - Check file sizes and versions

### ??? Troubleshooting

**Build Errors:**
- Ensure WiX Toolset is installed
- Verify KAIROS project builds successfully first
- Check that all referenced DLLs exist in the output directory

**Missing Files in MSI:**
- Update `Product.wxs` to include additional files
- Rebuild both KAIROS and KAIROS.Installer projects

**Installation Failures:**
- Check Windows Event Viewer > Application logs
- Review installation logs: `msiexec /i KaiROS-AI-Setup.msi /l*v install.log`

## Customization

### Changing Installation Directory
The default installation directory is:
- `C:\Program Files\Avnish Kumar\KaiROS AI` (x64)
- `C:\Program Files (x86)\Avnish Kumar\KaiROS AI` (x86)

Users can change this during interactive installation.

### Adding Files
To add additional files to the installer:
1. Open `Product.wxs`
2. Add `<File>` elements to the appropriate `<Component>` or create new components
3. Rebuild the installer

### Version Updates
Update the version in:
1. `Product.wxs` - `ProductVersion` variable
2. `Package.appxmanifest` - Version attribute
3. `KAIROS.csproj` - AssemblyVersion (if present)

## Code Signing

For production releases, sign the MSI:

```cmd
signtool sign /f YourCertificate.pfx /p YourPassword /t http://timestamp.digicert.com KaiROS-AI-Setup.msi
```

Microsoft Store may require signed packages depending on your submission type.

## Support

For issues with the installer:
- GitHub: https://github.com/avikeid2007/KaiROS
- Review WiX documentation: https://wixtoolset.org/documentation/

## References

- [Microsoft Store MSI App Certification Process](https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/app-certification-process)
- [Manual Package Validation](https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/manual-package-validation)
- [WiX Toolset Documentation](https://wixtoolset.org/documentation/)
