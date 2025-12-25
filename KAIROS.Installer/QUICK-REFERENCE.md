# Quick Reference - MSI Commands

## Build Commands

```powershell
# Build MSI (x64)
.\Build-MSI.ps1 -Platform x64 -Configuration Release

# Build MSI (x86)
.\Build-MSI.ps1 -Platform x86 -Configuration Release

# Build with code signing
.\Build-MSI.ps1 -Platform x64 -Configuration Release -Sign -CertificatePath "cert.pfx"

# Skip application build (MSI only)
.\Build-MSI.ps1 -Platform x64 -Configuration Release -SkipBuild
```

## Validation Commands

```powershell
# Validate MSI
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"

# Validate with cleanup
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi" -CleanupAfterTest

# Skip installation test
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi" -SkipInstallTest
```

## Installation Commands

```powershell
# Interactive install
msiexec /i KaiROS-AI-Setup.msi

# Silent install
msiexec /i KaiROS-AI-Setup.msi /quiet /qn

# Silent install with log
msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log

# Install to custom directory (interactive)
msiexec /i KaiROS-AI-Setup.msi INSTALLFOLDER="C:\CustomPath\KaiROS"
```

## Uninstallation Commands

```powershell
# Interactive uninstall
msiexec /x KaiROS-AI-Setup.msi

# Silent uninstall
msiexec /x KaiROS-AI-Setup.msi /quiet /qn

# Silent uninstall with log
msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log

# Uninstall by product code
msiexec /x {PRODUCT-GUID} /quiet /qn
```

## Extract/Admin Install

```powershell
# Extract MSI contents without installing
msiexec /a KaiROS-AI-Setup.msi /qn TARGETDIR="C:\Temp\Extract"

# Extract with log
msiexec /a KaiROS-AI-Setup.msi /qn TARGETDIR="C:\Temp\Extract" /l*v extract.log
```

## Upgrade Commands

```powershell
# Install new version (auto-upgrades)
msiexec /i KaiROS-AI-Setup-v1.0.2.msi /quiet /qn

# Force reinstall
msiexec /i KaiROS-AI-Setup.msi /qn REINSTALL=ALL REINSTALLMODE=vamus
```

## Diagnostic Commands

```powershell
# Get installed product info
Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*KaiROS*" }

# Get product code
Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -eq "KaiROS AI" } | Select-Object IdentifyingNumber

# Check installation path
Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*" | 
  Where-Object { $_.DisplayName -eq "KaiROS AI" } | 
  Select-Object DisplayName, DisplayVersion, InstallLocation

# View Windows Installer log
Get-EventLog -LogName Application -Source MsiInstaller -Newest 20

# Check digital signature
Get-AuthenticodeSignature KaiROS-AI-Setup.msi
```

## Code Signing Commands

```powershell
# Sign MSI with certificate
signtool sign /f "cert.pfx" /p "password" /t http://timestamp.digicert.com KaiROS-AI-Setup.msi

# Sign with certificate from store
signtool sign /n "Certificate Name" /t http://timestamp.digicert.com KaiROS-AI-Setup.msi

# Verify signature
signtool verify /pa KaiROS-AI-Setup.msi

# View signature details
Get-AuthenticodeSignature KaiROS-AI-Setup.msi | Format-List *
```

## WACK (Windows App Certification Kit)

```powershell
# Run WACK GUI
& "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcert.exe"

# Run WACK command line
& "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcertui.exe" `
  /ProductType "Desktop App" `
  /AppPath "C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe" `
  /ReportPath "C:\Temp\WACK-Report.xml"
```

## Exit Codes

| Code | Meaning |
|------|---------|
| 0 | Success |
| 1602 | User cancelled installation |
| 1603 | Fatal error during installation |
| 1618 | Another installation is already in progress |
| 1619 | Could not open the installation package |
| 1641 | Restart initiated after successful installation |
| 3010 | Restart required to complete installation |

## Environment Variables

```powershell
# Set log level for all MSI operations
$env:MSI_LOGGING = "voicewarmupx"

# Enable Windows Installer logging
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Installer" /v Logging /t REG_SZ /d "voicewarmupx" /f

# Disable logging
reg delete "HKLM\SOFTWARE\Policies\Microsoft\Windows\Installer" /v Logging /f
```

## Common File Paths

```
Main Application:
  C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe

Start Menu Shortcut:
  C:\ProgramData\Microsoft\Windows\Start Menu\Programs\KaiROS AI\

Desktop Shortcut:
  C:\Users\<username>\Desktop\KaiROS AI.lnk

Registry:
  HKCU\Software\Avnish Kumar\KaiROS AI
  HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{GUID}

Logs (if enabled):
  %TEMP%\MSI*.log
```

## MSBuild Commands (Advanced)

```powershell
# Build installer directly with MSBuild
msbuild KAIROS.Installer.wixproj /p:Configuration=Release /p:Platform=x64

# Clean and rebuild
msbuild KAIROS.Installer.wixproj /t:Clean,Build /p:Configuration=Release /p:Platform=x64

# Build with detailed logging
msbuild KAIROS.Installer.wixproj /p:Configuration=Release /p:Platform=x64 /v:detailed

# Build multiple platforms
msbuild KAIROS.Installer.wixproj /p:Configuration=Release /p:Platform=x64
msbuild KAIROS.Installer.wixproj /p:Configuration=Release /p:Platform=x86
```

## Troubleshooting

```powershell
# Enable verbose MSI logging for next operation
$env:MSI_LOGGING = "voicewarmupx"

# Check Windows Event Log
Get-EventLog -LogName Application -Source MsiInstaller -After (Get-Date).AddHours(-1)

# View current installations in progress
Get-Process msiexec

# Force kill stuck installation
Get-Process msiexec | Stop-Process -Force

# Clear Windows Installer cache (use with caution!)
# Not recommended - can break other installations
```

## Store Submission

```powershell
# Package for store submission
.\Build-MSI.ps1 -Platform x64 -Configuration Release -Sign -CertificatePath "cert.pfx"

# Validate before submission
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"

# Create submission package
$submissionFolder = "StoreSubmission"
New-Item -ItemType Directory -Path $submissionFolder -Force
Copy-Item "bin\x64\Release\KaiROS-AI-Setup.msi" -Destination "$submissionFolder\"
Copy-Item "SUBMISSION-GUIDE.md" -Destination "$submissionFolder\"
Copy-Item "bin\x64\Release\ValidationReport.txt" -Destination "$submissionFolder\"
Compress-Archive -Path "$submissionFolder\*" -DestinationPath "KaiROS-Store-Submission.zip"
```

---

For detailed information, see:
- [README.md](README.md)
- [SUBMISSION-GUIDE.md](SUBMISSION-GUIDE.md)
