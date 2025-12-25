# Changelog

All notable changes to the KaiROS AI MSI installer will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial MSI installer implementation
- Silent installation support for Microsoft Store
- Automated build and validation scripts
- GitHub Actions workflow for CI/CD
- Comprehensive documentation

## [1.0.2.0] - 2024-01-XX

### Added
- **MSI Installer Package**
  - WiX Toolset-based installer
  - Support for x64 and x86 platforms
  - Silent installation with `/quiet` and `/qn` switches
  - Proper upgrade handling with Major Upgrade
  - Desktop and Start Menu shortcuts
  
- **Build Automation**
  - `Build-MSI.ps1` - Automated build script
  - `Validate-MSI.ps1` - Validation and testing script
  - GitHub Actions workflow for automated builds
  - Code signing support
  
- **Documentation**
  - Complete Microsoft Store submission guide
  - Quick reference for common commands
  - Troubleshooting guide
  - Pre-submission checklist
  
- **Microsoft Store Compliance**
  - Silent installation/uninstallation support
  - Windows 10 1809+ compatibility
  - Per-machine installation scope
  - Clean registry and file removal
  - Standard Windows Installer exit codes

### Changed
- N/A (initial MSI release)

### Fixed
- N/A (initial MSI release)

### Security
- Optional code signing support for production releases

## Previous Versions

### [1.0.1.0] - Previous Release
- MSIX package only (no MSI)

### [1.0.0.0] - Initial Release
- MSIX package only (no MSI)

---

## Version Numbering

We use semantic versioning with four components: `MAJOR.MINOR.PATCH.BUILD`

- **MAJOR**: Incompatible API changes or major feature additions
- **MINOR**: New features, backward compatible
- **PATCH**: Bug fixes, backward compatible
- **BUILD**: Build number, auto-incremented

## Release Process

1. Update version in:
   - `KAIROS.Installer/Product.wxs` (`ProductVersion`)
   - `Package.appxmanifest` (`Version`)
   - This CHANGELOG.md

2. Build and test:
   ```powershell
   .\Build-MSI.ps1 -Platform x64 -Configuration Release
   .\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
   ```

3. Create Git tag:
   ```bash
   git tag -a v1.0.2.0 -m "Release version 1.0.2.0"
   git push origin v1.0.2.0
   ```

4. GitHub Actions will automatically:
   - Build MSI packages
   - Run validation tests
   - Create GitHub release
   - Prepare store submission package

5. Manual steps:
   - Download artifacts from GitHub Actions
   - Submit to Microsoft Store Partner Center
   - Monitor certification process

## Links

- [Microsoft Store Listing](https://www.microsoft.com/store/apps/KAIROS-AI)
- [GitHub Repository](https://github.com/avikeid2007/KaiROS)
- [Issue Tracker](https://github.com/avikeid2007/KaiROS/issues)
- [Partner Center](https://partner.microsoft.com/)
