# GitHub Actions CI/CD Setup Guide

This guide explains how to configure GitHub Actions for automated MSI builds.

## ?? What the Workflow Does

The GitHub Actions workflow (`.github/workflows/build-msi.yml`) automatically:

1. ? Builds MSI packages for x64 and x86
2. ? Runs validation tests
3. ? Signs packages (if certificate configured)
4. ? Creates GitHub releases
5. ? Generates store submission packages

## ?? Quick Start

The workflow is **already configured** and will run automatically when you:

### Trigger 1: Create a Version Tag
```bash
git tag v1.0.2
git push origin v1.0.2
```

### Trigger 2: Push to Main Branch
The workflow runs on pull requests to `main` branch (build only, no release).

### Trigger 3: Manual Trigger
Go to **Actions** tab in GitHub > **Build MSI Package** > **Run workflow**

## ?? Optional: Configure Code Signing

To enable automatic code signing in GitHub Actions, add signing secrets to your repository.

### Step 1: Prepare Certificate

You need a code signing certificate in `.pfx` format.

```powershell
# If you have separate .cer and .key files, convert to .pfx:
openssl pkcs12 -export -out certificate.pfx -inkey private.key -in certificate.cer

# You'll be prompted to set a password
```

### Step 2: Convert Certificate to Base64

```powershell
# Convert .pfx to base64 string
$certPath = "path\to\your\certificate.pfx"
$certBytes = [System.IO.File]::ReadAllBytes($certPath)
$certBase64 = [System.Convert]::ToBase64String($certBytes)

# Copy to clipboard
$certBase64 | Set-Clipboard

# Display (for manual copy if clipboard fails)
Write-Output $certBase64
```

### Step 3: Add Secrets to GitHub Repository

1. Go to your repository on GitHub
2. Navigate to: **Settings** > **Secrets and variables** > **Actions**
3. Click **New repository secret**
4. Add two secrets:

| Secret Name | Value |
|------------|-------|
| `SIGNING_CERTIFICATE` | The base64 string from Step 2 |
| `CERTIFICATE_PASSWORD` | Your certificate password |

### Step 4: Verify Configuration

After adding secrets:

1. Push a new tag:
   ```bash
   git tag v1.0.2
   git push origin v1.0.2
   ```

2. Go to **Actions** tab in GitHub

3. Watch the workflow run

4. Check the "Sign MSI" step - should show "MSI signed successfully"

5. Download the artifact and verify signature:
   ```powershell
   Get-AuthenticodeSignature KaiROS-AI-Setup.msi
   # Status should be: Valid
   ```

## ??? Workflow Customization

### Change Trigger Conditions

Edit `.github/workflows/build-msi.yml`:

```yaml
on:
  push:
    tags:
      - 'v*.*.*'          # Runs on version tags like v1.0.0
    branches:
      - main              # Also runs on main branch pushes
  pull_request:
    branches: [ main ]    # Runs on PRs to main
  workflow_dispatch:      # Allows manual trigger
```

### Add More Platforms

To build for x86 in addition to x64:

```yaml
strategy:
  matrix:
    platform: [x64, x86]  # Add or remove platforms here
```

### Change Build Configuration

```yaml
env:
  BUILD_CONFIGURATION: Release  # Change to Debug if needed
```

### Modify Artifact Retention

```yaml
- name: Upload MSI Artifact
  uses: actions/upload-artifact@v4
  with:
    retention-days: 30  # Change retention period (1-90 days)
```

## ?? Workflow Outputs

### For Each Build

After a successful workflow run, you'll find:

1. **Build Artifacts** (in Actions > Workflow run > Artifacts)
   - `KaiROS-AI-Setup-x64-v{version}.msi`
   - `KaiROS-AI-Setup-x86-v{version}.msi` (if enabled)
   - `ValidationReport-x64.txt`
   - `ValidationReport-x86.txt` (if enabled)

2. **GitHub Release** (for version tags only)
   - Automatic release created
   - MSI files attached
   - Validation reports attached
   - Release notes generated

3. **Store Submission Package** (for version tags only)
   - `Microsoft-Store-Submission-v{version}.zip`
   - Contains MSI, validation report, and documentation
   - Ready to submit to Microsoft Partner Center

## ?? Monitoring Workflow

### View Workflow Runs

1. Go to **Actions** tab in GitHub
2. Click on **Build MSI Package**
3. See all workflow runs and their status

### Check Build Logs

1. Click on a specific workflow run
2. Expand build steps to see detailed logs
3. Look for errors in red

### Download Artifacts

1. Scroll to bottom of workflow run page
2. Click **Artifacts** section
3. Download ZIP files
4. Extract and test MSI

## ?? Troubleshooting

### Workflow Fails to Run

**Problem**: Workflow doesn't trigger when pushing tags

**Solution**: 
- Ensure you pushed the tag: `git push origin v1.0.2`
- Check workflow file is in `.github/workflows/` directory
- Verify workflow file is valid YAML

### WiX Installation Fails

**Problem**: "WiX Toolset not found" error

**Solution**: The workflow automatically installs WiX. If it fails:
- Check if WiX download URL is accessible
- Review the "Install WiX Toolset" step logs
- May need to update WiX download URL in workflow file

### Signing Fails

**Problem**: "Code signing failed" error

**Possible Causes**:
1. Certificate not in correct format (must be .pfx)
2. Certificate password incorrect
3. Base64 encoding issue
4. Certificate expired

**Solutions**:
- Verify secrets are set correctly in repository settings
- Re-encode certificate to base64
- Check certificate validity: `Get-PfxCertificate cert.pfx`
- Ensure certificate is for code signing purposes

### Build Succeeds but MSI Missing

**Problem**: Workflow completes but no MSI artifact

**Solution**:
- Check build logs for errors
- Verify KAIROS project builds successfully
- Check MSI output path matches workflow expectations
- Review MSBuild output in logs

### Release Not Created

**Problem**: Workflow runs but no GitHub release created

**Solution**:
- Ensure you pushed a tag (not just a commit): `git push origin v1.0.2`
- Tag must match pattern in workflow: `v*.*.*`
- Check "create-release" job logs
- Verify `GITHUB_TOKEN` has necessary permissions

## ?? Best Practices

### Version Tagging

Use semantic versioning:
```bash
# Format: vMAJOR.MINOR.PATCH
git tag v1.0.0    # First release
git tag v1.0.1    # Bug fixes
git tag v1.1.0    # New features
git tag v2.0.0    # Breaking changes
```

### Testing Before Release

1. Test on pull requests first
2. Verify build succeeds
3. Download and test artifacts
4. Then create version tag for release

### Updating Workflow

1. Make changes to `.github/workflows/build-msi.yml`
2. Commit and push to a branch
3. Test via pull request
4. Merge when verified

### Security

- ? **DO**: Store certificates in GitHub Secrets
- ? **DO**: Use strong certificate passwords
- ? **DO**: Rotate certificates regularly
- ? **DON'T**: Commit certificates to repository
- ? **DON'T**: Share certificate passwords
- ? **DON'T**: Use test certificates for production

## ?? Resources

### GitHub Actions Documentation
- [Workflows](https://docs.github.com/en/actions/using-workflows)
- [Secrets](https://docs.github.com/en/actions/security-guides/encrypted-secrets)
- [Artifacts](https://docs.github.com/en/actions/using-workflows/storing-workflow-data-as-artifacts)

### Project Documentation
- [Build Script](KAIROS.Installer/Build-MSI.ps1)
- [Validation Script](KAIROS.Installer/Validate-MSI.ps1)
- [Submission Guide](KAIROS.Installer/SUBMISSION-GUIDE.md)

## ? Verification Checklist

After configuring GitHub Actions:

- [ ] Workflow file exists at `.github/workflows/build-msi.yml`
- [ ] Secrets configured (if using code signing):
  - [ ] `SIGNING_CERTIFICATE` secret added
  - [ ] `CERTIFICATE_PASSWORD` secret added
- [ ] Test workflow:
  - [ ] Create test tag: `git tag v1.0.0-test && git push origin v1.0.0-test`
  - [ ] Workflow runs automatically
  - [ ] All steps complete successfully
  - [ ] MSI artifacts generated
  - [ ] Validation reports generated
- [ ] Optional: Test manual trigger
  - [ ] Go to Actions tab
  - [ ] Run workflow manually
  - [ ] Verify success

---

## ?? Success!

Once configured, your GitHub Actions workflow will:
- ?? Automatically build MSI packages on every release
- ?? Create GitHub releases with downloadable installers
- ? Validate packages before distribution
- ?? Sign packages (if configured)
- ?? Generate store submission packages

**No manual building required!** Just tag a version and push. ??

---

**Need Help?**
- GitHub Issues: https://github.com/avikeid2007/KaiROS/issues
- Workflow Documentation: [GitHub Actions Docs](https://docs.github.com/en/actions)
