# Microsoft Store MSI Submission Guide

This guide will help you prepare and submit your KaiROS AI MSI package to the Microsoft Store.

## Quick Start

### 1. Install Prerequisites

```powershell
# Install WiX Toolset v3.11 or later
# Download from: https://wixtoolset.org/releases/
```

### 2. Build the MSI

```powershell
# Navigate to the installer directory
cd KAIROS.Installer

# Build MSI for x64 (recommended)
.\Build-MSI.ps1 -Platform x64 -Configuration Release

# Or build for x86
.\Build-MSI.ps1 -Platform x86 -Configuration Release

# With code signing
.\Build-MSI.ps1 -Platform x64 -Configuration Release -Sign -CertificatePath "path\to\cert.pfx" -CertificatePassword "password"
```

### 3. Validate the MSI

```powershell
# Run validation tests
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"

# With installation test (requires admin)
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi" -CleanupAfterTest
```

## Microsoft Store Requirements

### ? Certification Requirements

Your MSI must meet these requirements for Microsoft Store approval:

#### 1. **Silent Installation Support** ?
- Must support `/quiet` and `/qn` switches
- No user interaction during silent install
- **Test**: `msiexec /i KaiROS-AI-Setup.msi /quiet /qn`

#### 2. **Silent Uninstallation Support** ?
- Must support silent uninstall
- Clean removal of all components
- **Test**: `msiexec /x KaiROS-AI-Setup.msi /quiet /qn`

#### 3. **Windows Version Requirements** ?
- Minimum: Windows 10 version 1809 (Build 17763)
- Properly declared in MSI

#### 4. **Installation Scope** ?
- Per-machine installation configured
- Requires administrator privileges

#### 5. **Upgrade Handling** ?
- Major upgrades properly configured
- Clean upgrade from previous versions
- Downgrade protection enabled

#### 6. **Exit Codes**
Your MSI must return standard Windows Installer exit codes:
- `0` - Success
- `1602` - User cancelled
- `1618` - Another installation in progress
- `1603` - Fatal error
- `1641` - Restart required (initiated)
- `3010` - Restart required (not initiated)

#### 7. **Registry and File Cleanup** ?
- All registry entries removed on uninstall
- All files removed on uninstall
- No orphaned data

### ?? Pre-Submission Checklist

Before submitting to Microsoft Store, complete this checklist:

#### Build and Packaging
- [ ] MSI built for target platform (x64 recommended)
- [ ] Version number updated in all locations
- [ ] Code signing certificate applied (recommended)
- [ ] All required files included in MSI
- [ ] Native DLLs (llama.dll, ggml*.dll) included

#### Testing
- [ ] Silent installation succeeds: `msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log`
- [ ] Application launches after installation
- [ ] All features work correctly
- [ ] Silent uninstallation succeeds: `msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log`
- [ ] All files and registry entries removed after uninstall
- [ ] Upgrade from previous version succeeds
- [ ] Tested on clean Windows 10 1809+ machine
- [ ] No Windows Defender SmartScreen warnings

#### Windows App Certification Kit (WACK)
- [ ] Install the application
- [ ] Run WACK: `C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcert.exe`
- [ ] Select installed application
- [ ] Run all certification tests
- [ ] Address any failures
- [ ] Save WACK report

#### Documentation
- [ ] Privacy policy prepared
- [ ] EULA/Terms of Service ready
- [ ] Store listing description written
- [ ] Screenshots prepared (min 1, max 10)
- [ ] App icon in required sizes

#### Partner Center Preparation
- [ ] Microsoft Partner Center account created
- [ ] App identity reserved
- [ ] Store listing information ready
- [ ] Age ratings determined
- [ ] Pricing tier selected

## Detailed Testing Procedures

### Manual Installation Test

1. **Prepare Test Environment**
   ```powershell
   # Use a clean Windows 10 1809+ VM or test machine
   # Ensure .NET 8 runtime is NOT pre-installed
   ```

2. **Interactive Installation**
   ```powershell
   # Double-click the MSI or run:
   msiexec /i KaiROS-AI-Setup.msi
   
   # Verify:
   # - Installation wizard appears
   # - Installation completes successfully
   # - Desktop shortcut created (optional)
   # - Start menu shortcut created
   # - Application launches from shortcuts
   ```

3. **Silent Installation**
   ```powershell
   # Run as administrator
   msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log
   
   # Wait for completion (check Task Manager for msiexec.exe)
   
   # Verify:
   # - Exit code is 0 (success)
   # - No errors in install.log
   # - Application files in C:\Program Files\Avnish Kumar\KaiROS AI\
   # - Start menu shortcuts created
   # - Application launches successfully
   ```

4. **Application Functionality**
   ```powershell
   # Launch the application
   & "C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe"
   
   # Test:
   # - UI loads correctly
   # - Can initialize LLM with model
   # - Chat functionality works
   # - Database operations work
   # - No crashes or errors
   ```

5. **Silent Uninstallation**
   ```powershell
   # Run as administrator
   msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log
   
   # Verify:
   # - Exit code is 0 (success)
   # - No errors in uninstall.log
   # - Application folder removed
   # - Start menu shortcuts removed
   # - Desktop shortcut removed (if created)
   # - Registry entries cleaned up
   ```

### Upgrade Test

1. **Install Previous Version**
   ```powershell
   # Install version 1.0.1.0
   msiexec /i KaiROS-AI-Setup-v1.0.1.msi /quiet /qn
   ```

2. **Verify Previous Version**
   ```powershell
   # Check installed version in Add/Remove Programs
   Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -eq "KaiROS AI" }
   ```

3. **Install Current Version**
   ```powershell
   # Install version 1.0.2.0 (current)
   msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v upgrade.log
   
   # Verify:
   # - Upgrade completes successfully
   # - Application version updated
   # - User data preserved (if applicable)
   # - Application launches correctly
   ```

### Windows App Certification Kit (WACK)

1. **Install Application**
   ```powershell
   msiexec /i KaiROS-AI-Setup.msi /quiet /qn
   ```

2. **Run WACK**
   ```powershell
   # Launch WACK GUI
   & "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcert.exe"
   
   # Or command line:
   & "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcertui.exe" `
     /ProductType "Desktop App" `
     /AppPath "C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe" `
     /ReportPath "C:\Temp\WACK-Report.xml"
   ```

3. **Review Results**
   - All tests must pass
   - Address any failures before submission
   - Common issues:
     - Missing AppX manifest (not applicable for desktop MSI)
     - Performance issues
     - Crash on startup
     - High memory usage

## Microsoft Partner Center Submission

### 1. Create Partner Center Account

1. Go to [Microsoft Partner Center](https://partner.microsoft.com/)
2. Sign in or create account
3. Enroll in Windows & Xbox program
4. Pay enrollment fee (one-time)

### 2. Reserve App Identity

1. Navigate to **Apps and games** > **New product**
2. Select **Desktop application**
3. Enter app name: **KaiROS AI**
4. Reserve name

### 3. Create App Submission

1. Click on your app
2. Click **Start your submission**
3. Complete all required sections:

#### Properties
- **Category**: Productivity
- **Subcategory**: AI & Machine Learning
- **System requirements**: 
  - OS: Windows 10 version 1809 or higher
  - Architecture: x64 (or x86)
  - Memory: 4 GB minimum, 8 GB recommended
  - Storage: 500 MB

#### Age Ratings
- Complete age rating questionnaire
- Typical rating: EVERYONE

#### Packages
- **Package type**: MSI installer
- Upload: `KaiROS-AI-Setup.msi`
- Platform: x64 (or x86)

#### Store Listings
- **Description**: Write compelling app description
- **Screenshots**: Upload 1-10 screenshots
- **App icon**: Upload required sizes
- **Keywords**: AI, Assistant, Chat, Local LLM
- **Privacy policy URL**: (required if you collect data)
- **Support contact**: Your email

#### Pricing and Availability
- **Markets**: Select target markets
- **Pricing**: Free or paid
- **Visibility**: Public or private

#### Additional Information
- **App installer settings**: Configure auto-update (optional)
- **Targeted release**: Immediate or scheduled

### 4. Submit for Certification

1. Review all sections (all must be complete)
2. Click **Submit for certification**
3. Wait for review (typically 1-3 business days)

### 5. Monitor Certification

1. Check Partner Center dashboard
2. Review any certification failures
3. Address issues and resubmit if needed

## Troubleshooting

### Build Issues

**Error: WiX Toolset not found**
- Install WiX Toolset v3.11+: https://wixtoolset.org/releases/
- Ensure WiX is in PATH

**Error: KAIROS.exe not found**
- Build KAIROS project first: `dotnet build KAIROS.csproj -c Release`
- Check output path matches WiX configuration

**Error: Missing DLL files**
- Verify DLLs folder contains all native libraries
- Check NuGet packages are restored
- Rebuild KAIROS project

### Installation Issues

**Error: 1603 (Fatal error)**
- Check install.log for details
- Common causes:
  - Insufficient permissions (run as admin)
  - Another version already installed
  - Corrupted MSI file
  - Missing dependencies

**Application doesn't launch after install**
- Check Windows Event Viewer for errors
- Verify all DLLs are present in install folder
- Test on clean machine without dev tools

**Shortcuts not created**
- Verify user has permissions
- Check Start Menu folder exists
- Review installation log

### Certification Issues

**WACK Failure: Application crash**
- Test on clean Windows 10 machine
- Check for missing dependencies
- Verify exception handling

**WACK Failure: Performance issues**
- Optimize startup time
- Reduce initial memory footprint
- Profile with Visual Studio profiler

**Store Rejection: Missing privacy policy**
- Add privacy policy URL
- Clearly state data collection practices

**Store Rejection: Incomplete metadata**
- Ensure all required fields completed
- Add screenshots (minimum 1)
- Provide complete description

## Support and Resources

### Microsoft Documentation
- [MSI App Certification Process](https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/app-certification-process)
- [Manual Package Validation](https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/manual-package-validation)
- [Partner Center Documentation](https://learn.microsoft.com/en-us/windows/apps/publish/)

### WiX Toolset
- [WiX Documentation](https://wixtoolset.org/documentation/)
- [WiX Tutorial](https://www.firegiant.com/wix/tutorial/)

### Project Resources
- GitHub: https://github.com/avikeid2007/KaiROS
- Issues: https://github.com/avikeid2007/KaiROS/issues

## Updating the MSI

When releasing a new version:

1. **Update Version Numbers**
   ```xml
   <!-- In Product.wxs -->
   <?define ProductVersion = "1.0.3.0" ?>
   
   <!-- In Package.appxmanifest -->
   <Identity Version="1.0.3.0" />
   ```

2. **Update Changelog** (create CHANGELOG.md if needed)

3. **Rebuild and Test**
   ```powershell
   .\Build-MSI.ps1 -Platform x64 -Configuration Release
   .\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
   ```

4. **Test Upgrade**
   - Install previous version
   - Install new version
   - Verify upgrade success

5. **Submit Update to Store**
   - Create new submission in Partner Center
   - Upload new MSI
   - Update release notes
   - Submit for certification

## Best Practices

### Version Numbering
- Use semantic versioning: MAJOR.MINOR.PATCH.BUILD
- Increment appropriately:
  - MAJOR: Breaking changes
  - MINOR: New features
  - PATCH: Bug fixes
  - BUILD: Build number (auto-increment)

### Code Signing
- Always sign production MSIs
- Use trusted certificate authority
- Timestamp signatures for long-term validity

### Testing
- Test on clean VMs
- Test multiple Windows versions (10, 11)
- Test with different user privileges
- Test network-restricted environments

### Documentation
- Keep README.md updated
- Document system requirements
- Provide troubleshooting guide
- Include known issues

### User Experience
- Fast installation (< 2 minutes)
- Clear progress indication
- Meaningful error messages
- Easy uninstallation

---

**Need Help?**
- File an issue: https://github.com/avikeid2007/KaiROS/issues
- Email: avikeid2007@gmail.com
