# GitHub Pages Setup Guide for KaiROS

This guide explains how to enable GitHub Pages to host your release downloads.

## What is GitHub Pages?

GitHub Pages is a free static site hosting service that allows you to publish websites directly from your GitHub repository. For KaiROS, we use it to:

- Host beautiful download pages for releases
- Provide direct download links for MSI installers
- Share store submission packages
- Create a user-friendly alternative to GitHub Releases

## One-Time Setup

### Step 1: Enable GitHub Pages

1. Go to your repository on GitHub: https://github.com/avikeid2007/KaiROS
2. Click **Settings** (in the repository menu)
3. In the left sidebar, click **Pages**
4. Under "Build and deployment":
   - **Source**: Select "GitHub Actions"
   - This allows the workflow to deploy automatically

### Step 2: Verify Workflow Permissions

1. In repository **Settings**, click **Actions** → **General**
2. Scroll to "Workflow permissions"
3. Ensure "Read and write permissions" is selected
4. Check "Allow GitHub Actions to create and approve pull requests"
5. Click **Save**

## How It Works

Once GitHub Pages is enabled, the automated workflow will:

1. **On Tag Push**: When you push a tag like `v1.0.3.0`:
   - Builds MSI packages
   - Creates GitHub Release
   - Generates HTML pages for downloads
   - Deploys to GitHub Pages automatically

2. **Pages Structure**:
   ```
   https://avikeid2007.github.io/KaiROS/
   ├── index.html (redirects to downloads/)
   └── downloads/
       ├── index.html (main downloads page)
       └── 1.0.3.0/
           ├── index.html (version-specific page)
           ├── KaiROS-AI-Setup-x64-v1.0.3.0.msi
           ├── KaiROS-AI-Setup-x86-v1.0.3.0.msi
           └── KaiROS-Store-Submission-v1.0.3.0.zip
   ```

3. **Access Points**:
   - Main: https://avikeid2007.github.io/KaiROS
   - Downloads: https://avikeid2007.github.io/KaiROS/downloads/
   - Version: https://avikeid2007.github.io/KaiROS/downloads/1.0.3.0/

## Testing the Setup

### Test with Existing Release

If you already have a release:

1. Go to [Actions](https://github.com/avikeid2007/KaiROS/actions)
2. Click "Build MSI Package"
3. Click "Run workflow"
4. Enter a version number (e.g., "1.0.2.0")
5. Check "Create GitHub release"
6. Click "Run workflow"
7. Wait for deployment to complete
8. Visit https://avikeid2007.github.io/KaiROS

### Create a New Release

Follow the [Release Guide](RELEASE_GUIDE.md):

```bash
# Update version numbers
# Commit changes
git add Package.appxmanifest KAIROS.Installer/Product.wxs
git commit -m "Bump version to 1.0.3.0"
git push origin main

# Create and push tag
git tag -a v1.0.3.0 -m "Release version 1.0.3.0"
git push origin v1.0.3.0

# Wait for workflow to complete
# Visit https://avikeid2007.github.io/KaiROS
```

## Customizing the Pages

The download pages are generated automatically in the workflow. To customize:

### Modify Page Design

Edit `.github/workflows/build-msi.yml` in the `deploy-to-pages` job:

```yaml
- name: Create Downloads Page
  run: |
    # Modify the HTML/CSS in this section
    $versionPage = @"
    <!DOCTYPE html>
    ...
    "@
```

### Change Colors/Styling

The default theme uses a purple gradient. To change:

1. Find the `<style>` section in the workflow
2. Modify the CSS:
   - Background gradient: `background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);`
   - Button colors: `.download-btn { background: ... }`
   - Other styling as needed

### Add Custom Content

You can add:
- Release notes
- Screenshots
- Feature highlights
- Installation videos
- Support links

## Troubleshooting

### Pages Not Deploying

**Check workflow status**:
1. Go to [Actions](https://github.com/avikeid2007/KaiROS/actions)
2. Find the failed workflow run
3. Check the `deploy-to-pages` job logs

**Common issues**:
- GitHub Pages not enabled in Settings → Pages
- Workflow lacks permissions (Settings → Actions → General)
- Branch protection rules preventing deployment

### 404 Error When Visiting Pages

**Wait for deployment**:
- First deployment can take 5-10 minutes
- Check Actions for deployment status

**Verify URL**:
- Correct: `https://avikeid2007.github.io/KaiROS`
- Incorrect: `https://avikeid2007.github.io/kairos` (lowercase)

### Files Not Downloading

**Check artifact availability**:
1. Go to workflow run
2. Verify artifacts were uploaded
3. Check download links in generated HTML

**CORS issues**:
- GitHub Pages serves files correctly by default
- Ensure files are in the `pages/` directory

### Old Version Still Showing

**Clear cache**:
- Hard refresh: `Ctrl+F5` (Windows) or `Cmd+Shift+R` (Mac)
- Clear browser cache
- Try incognito/private browsing

**Check deployment**:
- Each new release should deploy automatically
- Verify the workflow ran successfully

## Advanced Configuration

### Custom Domain

To use a custom domain (e.g., `downloads.kairos.ai`):

1. Go to Settings → Pages
2. Enter your custom domain
3. Add DNS records:
   ```
   Type: CNAME
   Name: downloads
   Value: avikeid2007.github.io
   ```
4. Wait for DNS propagation (can take 24-48 hours)

### HTTPS

GitHub Pages provides free HTTPS:
- Automatically enabled for `*.github.io` domains
- Custom domains get free Let's Encrypt certificates
- No additional configuration needed

### Multiple Versions

The workflow automatically creates version-specific pages:
- Each tag creates a new version folder
- Main downloads page lists all versions
- Old versions remain accessible

## Monitoring

### Check Deployment Status

```bash
# Using GitHub CLI
gh run list --workflow=build-msi.yml

# Check specific run
gh run view <run-id>

# View deployment logs
gh run view <run-id> --log
```

### Analytics

To track downloads:

1. Add Google Analytics to the HTML templates
2. Use GitHub's traffic insights (limited data)
3. Monitor workflow artifacts download count

## Security

### File Integrity

- MSI files are built in GitHub Actions
- Source code is version controlled
- Deployment uses GitHub's infrastructure

### Code Signing

For production releases:
- Configure `SIGNING_CERTIFICATE` secret
- Add `CERTIFICATE_PASSWORD` secret
- Workflow automatically signs MSI files

## Maintenance

### Cleanup Old Versions

GitHub Pages has no file limit, but you can clean up old versions:

1. Modify the workflow to limit stored versions
2. Add a cleanup step:
   ```powershell
   # Keep only last 5 versions
   Get-ChildItem "pages/downloads" | 
     Where-Object { $_.PSIsContainer } |
     Sort-Object CreationTime -Descending |
     Select-Object -Skip 5 |
     Remove-Item -Recurse
   ```

### Update Main Page

The main downloads page shows only the latest version by default. To show multiple:

1. Modify the workflow's `Create Downloads Page` step
2. Add logic to list multiple versions
3. Generate links for each version

## Resources

- [GitHub Pages Documentation](https://docs.github.com/pages)
- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Deploy Pages Action](https://github.com/actions/deploy-pages)
- [Upload Pages Artifact](https://github.com/actions/upload-pages-artifact)

## Support

If you encounter issues:

1. Check [Actions](https://github.com/avikeid2007/KaiROS/actions) for errors
2. Review [Issues](https://github.com/avikeid2007/KaiROS/issues)
3. File a new issue with:
   - Workflow run link
   - Error messages
   - Expected vs actual behavior

---

**Quick Start Checklist**:
- [ ] Enable GitHub Pages in Settings → Pages → Source: GitHub Actions
- [ ] Set workflow permissions in Settings → Actions → General
- [ ] Create and push a tag: `git tag -a v1.0.3.0 -m "Release" && git push origin v1.0.3.0`
- [ ] Wait for workflow to complete
- [ ] Visit https://avikeid2007.github.io/KaiROS
- [ ] Share the downloads link with users!
