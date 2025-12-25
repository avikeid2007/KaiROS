# ?? MSI Quick Reference Card - Keep This Handy!

## ? Common Commands

### Build MSI
```powershell
cd F:\source\KaiROS\KAIROS.Installer
.\Build-MSI.ps1 -Platform x64 -Configuration Release
```

### Validate MSI
```powershell
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
```

### Install (Silent)
```powershell
msiexec /i bin\x64\Release\KaiROS-AI-Setup.msi /quiet /qn /l*v install.log
```

### Uninstall (Silent)
```powershell
msiexec /x bin\x64\Release\KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log
```

## ?? Important Paths

| Item | Path |
|------|------|
| **MSI Output** | `F:\source\KaiROS\KAIROS.Installer\bin\x64\Release\KaiROS-AI-Setup.msi` |
| **Install Location** | `C:\Program Files\Avnish Kumar\KaiROS AI\` |
| **WiX Definition** | `F:\source\KaiROS\KAIROS.Installer\Product.wxs` |
| **Build Script** | `F:\source\KaiROS\KAIROS.Installer\Build-MSI.ps1` |

## ?? Documentation Map

| Need | Document |
|------|----------|
| ?? **Get Started** | [GET-STARTED-WITH-MSI.md](GET-STARTED-WITH-MSI.md) |
| ?? **Build & Install** | [KAIROS.Installer/README.md](KAIROS.Installer/README.md) |
| ?? **Submit to Store** | [KAIROS.Installer/SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md) |
| ?? **All Commands** | [KAIROS.Installer/QUICK-REFERENCE.md](KAIROS.Installer/QUICK-REFERENCE.md) |
| ? **Progress Tracker** | [MSI-CHECKLIST.md](MSI-CHECKLIST.md) |

## ?? Pre-Submission Checklist

- [ ] WiX Toolset installed
- [ ] MSI builds successfully
- [ ] Validation passes
- [ ] Tested on clean Windows 10 1809+
- [ ] WACK certification passes
- [ ] Store listing prepared

## ?? Version Update Process

1. Edit `KAIROS.Installer/Product.wxs`:
   ```xml
   <?define ProductVersion = "1.0.3.0" ?>
   ```

2. Edit `Package.appxmanifest`:
   ```xml
   <Identity Version="1.0.3.0" />
   ```

3. Rebuild MSI

## ?? Troubleshooting

| Error | Solution |
|-------|----------|
| **WiX not found** | Install from https://wixtoolset.org/releases/ |
| **Build fails** | Build KAIROS.csproj first |
| **Install 1603** | Run as Administrator |
| **App won't launch** | Check Event Viewer |

## ?? Important Links

- **WiX Toolset**: https://wixtoolset.org/releases/
- **Partner Center**: https://partner.microsoft.com/
- **Store Certification**: https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/app-certification-process
- **GitHub Repo**: https://github.com/avikeid2007/KaiROS

## ?? Support

- **GitHub Issues**: https://github.com/avikeid2007/KaiROS/issues
- **Email**: avikeid2007@gmail.com

---

**Tip**: Pin this file for quick access! ??
