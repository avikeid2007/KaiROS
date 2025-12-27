# KaiROS Release Guide

This guide explains how to create releases for KaiROS AI, including building installers and preparing packages for Microsoft Store submission.

## Table of Contents
- [Understanding Releases](#understanding-releases)
- [Creating a Release](#creating-a-release)
- [Automated Build Process](#automated-build-process)
- [Manual Release Process](#manual-release-process)
- [Microsoft Store Submission](#microsoft-store-submission)
- [Troubleshooting](#troubleshooting)

## Understanding Releases

KaiROS uses a tag-based release system:
- **Tags** trigger automated builds and releases
- **MSI installers** are built for x64 and x86 architectures
- **GitHub Releases** are created automatically with installers attached
- **Store submission packages** are prepared for Microsoft Store

## Creating a Release

### Prerequisites

1. Ensure all code changes are committed and pushed to the `main` branch
2. All tests pass
3. Version numbers are updated in:
   - `Package.appxmanifest` (line 13: `Version="X.X.X.0"`)
   - `KAIROS.Installer/Product.wxs` (ProductVersion define)

### Step-by-Step Process

#### 1. Update Version Numbers

```powershell
# Example: Updating to version 1.0.3.0

# Update Package.appxmanifest
$manifestPath = "Package.appxmanifest"
$manifest = Get-Content $manifestPath -Raw
$manifest = $manifest -replace 'Version="[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+"', 'Version="1.0.3.0"'
Set-Content -Path $manifestPath -Value $manifest

# Update Product.wxs
$productPath = "KAIROS.Installer\Product.wxs"
$product = Get-Content $productPath -Raw
$product = $product -replace '<?define ProductVersion = "[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+" ?>', '<?define ProductVersion = "1.0.3.0" ?>'
Set-Content -Path $productPath -Value $product
```

#### 2. Commit Version Changes

```bash
git add Package.appxmanifest KAIROS.Installer/Product.wxs
git commit -m "Bump version to 1.0.3.0"
git push origin main
```

#### 3. Create and Push Tag

```bash
# Create annotated tag
git tag -a v1.0.3.0 -m "Release version 1.0.3.0"

# Push tag to trigger release workflow
git push origin v1.0.3.0
```

#### 4. Monitor Release Build

1. Go to [GitHub Actions](https://github.com/avikeid2007/KaiROS/actions)
2. Find the "Build MSI Package" workflow triggered by your tag
3. Monitor the build progress
4. Verify all jobs complete successfully:
   - `build-msi` (for x64 and x86)
   - `create-release`
   - `prepare-store-submission`
   - `deploy-to-pages`

### What Gets Created

When you push a tag, the following artifacts are automatically created:

1. **MSI Installers** (uploaded as artifacts and release assets):
   - `KaiROS-AI-Setup-x64-vX.X.X.X.msi` - 64-bit installer
   - `KaiROS-AI-Setup-x86-vX.X.X.X.msi` - 32-bit installer

2. **GitHub Release**:
   - Created automatically at https://github.com/avikeid2007/KaiROS/releases
   - Includes both MSI installers
   - Contains auto-generated release notes

3. **GitHub Pages Deployment**:
   - Downloads page deployed to https://avikeid2007.github.io/KaiROS
   - User-friendly download interface
   - Automatic redirect from root to downloads page
   - Version-specific download pages

4. **Store Submission Package**:
   - `Microsoft-Store-Submission-vX.X.X.X.zip`
   - Contains MSI, documentation, and submission checklist
   - Available in workflow artifacts for 90 days
   - Also available on GitHub Pages downloads

## Automated Build Process

The GitHub Actions workflow automatically:

1. **Builds MSI packages** for x64 and x86:
   - Restores NuGet packages
   - Installs WiX Toolset
   - Runs `Build-MSI.ps1` script
   - Validates MSI packages

2. **Signs MSI** (if certificate is configured):
   - Uses `SIGNING_CERTIFICATE` and `CERTIFICATE_PASSWORD` secrets
   - Only signs when pushing tags (release builds)

3. **Creates GitHub Release**:
   - Downloads built MSI packages
   - Generates release notes
   - Uploads MSI files as release assets

4. **Prepares Store Submission**:
   - Bundles MSI, documentation, and checklists
   - Creates submission package ZIP
   - Uploads as workflow artifact

5. **Deploys to GitHub Pages**:
   - Creates user-friendly download pages
   - Publishes MSI installers and store submission package
   - Updates main downloads index
   - Accessible at https://avikeid2007.github.io/KaiROS

## Accessing Downloads

### For End Users

After a release is created, users can download from multiple locations:

1. **GitHub Pages** (Easiest):
   - Visit https://avikeid2007.github.io/KaiROS
   - Click on the desired version
   - Download MSI installer directly from the browser

2. **GitHub Releases**:
   - Visit https://github.com/avikeid2007/KaiROS/releases
   - Find the desired version
   - Download MSI from release assets

### For Developers/Store Submission

1. **GitHub Actions Artifacts** (90-day retention):
   - Go to the [workflow run](https://github.com/avikeid2007/KaiROS/actions)
   - Scroll to "Artifacts" section
   - Download `Microsoft-Store-Submission-vX.X.X.X.zip`

2. **GitHub Pages**:
   - Visit https://avikeid2007.github.io/KaiROS/downloads/X.X.X.X/
   - Download the store submission package

## Manual Release Process

If you need to create a release manually without using tags:

### Option 1: Use Workflow Dispatch

1. Go to [Actions](https://github.com/avikeid2007/KaiROS/actions/workflows/build-msi.yml)
2. Click "Run workflow"
3. Select branch: `main`
4. Click "Run workflow"

**Note:** This builds MSI packages but does NOT create a GitHub release or store submission package (those require tags).

### Option 2: Local Build

```powershell
# Navigate to installer directory
cd KAIROS.Installer

# Build x64 MSI
.\Build-MSI.ps1 -Platform x64 -Configuration Release

# Build x86 MSI
.\Build-MSI.ps1 -Platform x86 -Configuration Release

# Validate MSI
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"

# Sign MSI (optional)
$certPath = "path\to\certificate.pfx"
$msiPath = "bin\x64\Release\KaiROS-AI-Setup.msi"
& "C:\Program Files (x86)\Windows Kits\10\bin\*\x64\signtool.exe" sign /f $certPath /p "password" /t http://timestamp.digicert.com $msiPath
```

## Microsoft Store Submission

### Getting the Submission Package

After pushing a tag and waiting for the workflow to complete:

1. Go to [Actions](https://github.com/avikeid2007/KaiROS/actions)
2. Click on the workflow run triggered by your tag
3. Scroll to "Artifacts" section
4. Download `Microsoft-Store-Submission-vX.X.X.X.zip`

### Package Contents

The submission package includes:
- `KaiROS-AI-Setup.msi` - MSI installer for Microsoft Store
- `ValidationReport.txt` - Automated validation results
- `SUBMISSION-GUIDE.md` - Detailed MSI submission guide
- `QUICK-REFERENCE.md` - Quick command reference
- `README.md` - Project documentation
- `SUBMISSION-CHECKLIST.txt` - Pre-submission checklist

### Submitting to Microsoft Store

1. **Extract the submission package**

2. **Review the checklist**:
   - Open `SUBMISSION-CHECKLIST.txt`
   - Complete all pre-submission tasks

3. **Test the MSI**:
   ```powershell
   # Silent install test
   msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install.log
   
   # Verify installation
   & "C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe"
   
   # Silent uninstall test
   msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log
   ```

4. **Log in to Partner Center**:
   - Go to [Microsoft Partner Center](https://partner.microsoft.com/dashboard)
   - Navigate to your KaiROS AI app (or create new submission)

5. **Upload MSI package**:
   - In the "Packages" section
   - Upload `KaiROS-AI-Setup.msi`
   - Select platform: x64

6. **Complete store listing**:
   - Description, screenshots, etc.
   - Refer to `SUBMISSION-GUIDE.md` for details

7. **Submit for certification**

### Microsoft Store Accepts MSI

✅ **Good News**: Microsoft Store accepts MSI installers directly!

You don't need to convert to MSIX. The MSI package built by the workflow is ready for Microsoft Store submission.

## Troubleshooting

### Tag Already Exists

```bash
# Delete local tag
git tag -d v1.0.3.0

# Delete remote tag
git push origin :refs/tags/v1.0.3.0

# Create new tag
git tag -a v1.0.3.0 -m "Release version 1.0.3.0"
git push origin v1.0.3.0
```

### Workflow Not Triggered

1. Verify tag name starts with `v` (e.g., `v1.0.3.0`)
2. Check GitHub Actions are enabled for repository
3. Verify workflow file exists in `.github/workflows/build-msi.yml`

### Build Failures

1. Check [Actions](https://github.com/avikeid2007/KaiROS/actions) for error logs
2. Common issues:
   - Version mismatch between files
   - Missing NuGet packages
   - WiX Toolset installation failure

### No Release Created

If MSI builds succeed but no release is created:
1. Verify you pushed a **tag** (not just a commit)
2. Check `create-release` job in workflow run
3. Ensure `GITHUB_TOKEN` has necessary permissions

### Missing Store Submission Package

If `prepare-store-submission` job fails:
1. Check that all required files exist:
   - `KAIROS.Installer/SUBMISSION-GUIDE.md`
   - `KAIROS.Installer/QUICK-REFERENCE.md`
   - `README.md`
2. Verify artifact uploads succeeded

## Version Numbering

Follow semantic versioning: `MAJOR.MINOR.PATCH.BUILD`

- **MAJOR**: Breaking changes (e.g., 2.0.0.0)
- **MINOR**: New features (e.g., 1.1.0.0)
- **PATCH**: Bug fixes (e.g., 1.0.1.0)
- **BUILD**: Build number, usually 0 for releases

Example progression:
- `1.0.0.0` - Initial release
- `1.0.1.0` - Bug fix release
- `1.1.0.0` - New feature release
- `2.0.0.0` - Major version with breaking changes

## Quick Reference

### Create a new release:
```bash
# 1. Update versions in Package.appxmanifest and Product.wxs
# 2. Commit changes
git add Package.appxmanifest KAIROS.Installer/Product.wxs
git commit -m "Bump version to X.X.X.0"
git push origin main

# 3. Create and push tag
git tag -a vX.X.X.0 -m "Release version X.X.X.0"
git push origin vX.X.X.0

# 4. Wait for automated build and release
```

### Check release status:
```bash
# View releases
gh release list

# View workflow runs
gh run list --workflow=build-msi.yml

# Download release assets
gh release download vX.X.X.0
```

## Additional Resources

- [MSI Submission Guide](KAIROS.Installer/SUBMISSION-GUIDE.md)
- [Microsoft Store Guide](MICROSOFT_STORE_GUIDE.md)
- [Build Status](KAIROS.Installer/BUILD-STATUS.md)
- [GitHub Actions Workflow](.github/workflows/build-msi.yml)
- [Microsoft Partner Center](https://partner.microsoft.com/dashboard)

---

**Questions or Issues?**
- File an issue: https://github.com/avikeid2007/KaiROS/issues
- Contact: avikeid2007@gmail.com
