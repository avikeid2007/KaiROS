# GitHub Actions Setup for MSI Build

This document explains how the GitHub Actions workflow is configured to automatically build and publish your MSI installer.

## Workflow Overview

The workflow is defined in `.github/workflows/build-msi.yml` and runs on:

1. **Tag pushes**: When you create a tag like `v1.0.2.0`
2. **Pull requests**: To test MSI builds on PRs to main branch
3. **Manual trigger**: Via GitHub Actions UI (workflow_dispatch)

## Build Matrix

The workflow builds MSI installers for both platforms:
- **x64** (64-bit) - Recommended for most users
- **x86** (32-bit) - For older systems

## Workflow Jobs

### 1. `build-msi`

Builds the MSI installer for each platform.

**Steps:**
1. **Checkout code** - Gets your repository
2. **Setup .NET** - Installs .NET 8 SDK
3. **Setup MSBuild** - Required for WiX compilation
4. **Install WiX Toolset** - Downloads and installs WiX 3.11
5. **Restore NuGet packages** - Downloads dependencies
6. **Build and Package MSI** - Runs `Build-MSI.ps1` script
7. **Sign MSI** (optional) - Signs the MSI if certificate is configured
8. **Upload artifacts** - Uploads MSI and validation report

**Artifacts produced:**
- `KaiROS-AI-Setup-x64-v{version}.msi`
- `KaiROS-AI-Setup-x86-v{version}.msi`
- Validation reports

### 2. `create-release`

Creates a GitHub Release with the MSI installers (only on tag pushes).

**Steps:**
1. Downloads all MSI artifacts
2. Creates release notes
3. Creates GitHub Release
4. Attaches MSI files and validation reports

### 3. `prepare-store-submission`

Prepares a package for Microsoft Store submission (only on tag pushes).

**Artifacts produced:**
- ZIP file containing:
  - x64 MSI installer
  - Validation report
  - Submission guides
  - Checklist

## Configuration

### Environment Variables

```yaml
env:
  SOLUTION_PATH: KAIROS.csproj
  INSTALLER_PATH: KAIROS.Installer\KAIROS.Installer.wixproj
  BUILD_CONFIGURATION: Release
```

### Secrets (Optional for Code Signing)

To enable MSI code signing, configure these secrets in your GitHub repository:

1. Go to **Settings** ? **Secrets and variables** ? **Actions**
2. Add the following secrets:

| Secret Name | Description | Required |
|-------------|-------------|----------|
| `SIGNING_CERTIFICATE` | Base64-encoded PFX certificate | No |
| `CERTIFICATE_PASSWORD` | Password for the PFX certificate | No |

**How to encode your certificate:**

```powershell
$certPath = "path\to\your\certificate.pfx"
$certBytes = [System.IO.File]::ReadAllBytes($certPath)
$certBase64 = [System.Convert]::ToBase64String($certBytes)
$certBase64 | Set-Clipboard
# Now paste into GitHub Secrets
```

## Triggering a Build

### Automatic Triggers

**1. Create a Tag (Recommended for Releases)**

```bash
git tag v1.0.2.0
git push origin v1.0.2.0
```

This will:
- ? Build MSI for x64 and x86
- ? Create GitHub Release
- ? Prepare Microsoft Store submission package
- ? Sign MSI (if certificate configured)

**2. Create Pull Request**

When you create a PR to the `main` branch:
- ? Builds MSI to verify it compiles
- ? Does not create release
- ? Does not sign MSI

### Manual Trigger

1. Go to **Actions** tab in GitHub
2. Select **Build MSI Package** workflow
3. Click **Run workflow**
4. Select branch
5. Click **Run workflow** button

## Build Output

### On Pull Requests

- MSI artifacts uploaded (30 day retention)
- Can download from Actions ? Workflow run ? Artifacts

### On Tag Pushes

- GitHub Release created automatically
- MSI files attached to release
- Microsoft Store submission package available as artifact

## Troubleshooting

### Build Fails at WiX Installation

**Issue:** WiX download or installation fails

**Solution:**
- GitHub Actions downloads WiX 3.11 from official GitHub releases
- If download fails, check internet connectivity or try re-running the workflow

### Build Fails at MSI Build Step

**Issue:** `Build-MSI.ps1` script fails

**Solution:**
1. Check the build logs for specific error
2. Verify all files exist in repository:
   - `KAIROS.Installer/Build-MSI.ps1`
   - `KAIROS.Installer/Product.wxs`
   - `KAIROS.Installer/KAIROS.Installer.wixproj`
3. Test locally using the same script

### Signing Fails

**Issue:** MSI signing step fails

**Solutions:**
- Verify `SIGNING_CERTIFICATE` secret is valid base64
- Verify `CERTIFICATE_PASSWORD` is correct
- Ensure certificate is not expired
- Check that certificate is in PFX format

### Release Not Created

**Issue:** GitHub Release not created after tag push

**Possible causes:**
1. Tag doesn't match pattern `v*.*.*` (e.g., must be `v1.0.0`)
2. Build failed before release step
3. Insufficient permissions

**Solution:**
- Ensure workflow has `contents: write` permission (already configured)
- Check tag format: `git tag -l`
- Review build logs for errors

## Local Testing

Before pushing tags, test the build locally:

```powershell
# Test x64 build
cd F:\source\KaiROS\KAIROS.Installer
.\Build-MSI.ps1 -Platform x64 -Configuration Release

# Test x86 build
.\Build-MSI.ps1 -Platform x86 -Configuration Release
```

## Customization

### Change MSI Filename

Edit `.github/workflows/build-msi.yml`:

```yaml
- name: Upload MSI Artifact
  uses: actions/upload-artifact@v4
  with:
    name: YourCustomName-${{ matrix.platform }}-v${{ steps.version.outputs.version }}
```

### Add Additional Platforms

Edit the matrix in `.github/workflows/build-msi.yml`:

```yaml
strategy:
  matrix:
    platform: [x64, x86, ARM64]  # Add ARM64
```

Note: Ensure your project supports the platform.

### Change Artifact Retention

Default is 30 days for builds, 90 days for store submissions.

```yaml
- name: Upload MSI Artifact
  uses: actions/upload-artifact@v4
  with:
    retention-days: 60  # Change to 60 days
```

## Best Practices

1. **Always test locally first** before creating tags
2. **Use semantic versioning** for tags (v1.0.0, v1.1.0, v2.0.0)
3. **Review validation reports** before submitting to Microsoft Store
4. **Keep secrets secure** - never commit certificates or passwords
5. **Update version numbers** in both:
   - `Product.wxs` ? `ProductVersion`
   - Tag name (they should match)

## Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [WiX Toolset Documentation](https://wixtoolset.org/documentation/)
- [Microsoft Store Submission Guide](SUBMISSION-GUIDE.md)
- [Build Script Reference](README.md)

## Support

If you encounter issues with the GitHub Actions workflow:

1. Check the **Actions** tab for detailed logs
2. Review this documentation
3. Test the build script locally
4. Open an issue on GitHub with:
   - Workflow run URL
   - Error messages from logs
   - Steps to reproduce
