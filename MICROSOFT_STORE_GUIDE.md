# Microsoft Store Publishing Guide for KAIROS

## ✅ Current Status
Your project is **ready for Microsoft Store submission** with MSI packages.

## 📦 What Package to Submit

### Microsoft Store Accepts MSI Installers! ✅

Good news: You can submit MSI installers directly to the Microsoft Store. You **do not** need to convert to MSIX format.

The automated build workflow (`build-msi.yml`) already creates:
- **MSI packages** for x64 and x86 architectures
- **Store submission package** with all necessary files and documentation

## 🚀 Quick Start: Creating a Release for MS Store

### Step 1: Create a Release Tag

```bash
# Update version numbers in your code first
# Then create and push a tag:
git tag -a v1.0.3.0 -m "Release version 1.0.3.0"
git push origin v1.0.3.0
```

### Step 2: Download Store Submission Package

1. Go to [GitHub Actions](https://github.com/avikeid2007/KaiROS/actions)
2. Click on the workflow run triggered by your tag
3. Download artifact: `Microsoft-Store-Submission-vX.X.X.X.zip`

### Step 3: Submit to Microsoft Store

1. Extract the submission package
2. Review `SUBMISSION-CHECKLIST.txt`
3. Test the MSI installer
4. Log in to [Microsoft Partner Center](https://partner.microsoft.com/dashboard)
5. Upload `KaiROS-AI-Setup.msi` from the package
6. Complete store listing and submit for certification

**For detailed instructions, see [RELEASE_GUIDE.md](RELEASE_GUIDE.md)**

## 🎯 Why MSI Works for Microsoft Store

Microsoft Store accepts MSI installers for desktop applications. Your MSI packages include:
- Silent installation support (`/quiet /qn`)
- Proper upgrade handling
- Windows 10/11 compatibility
- All necessary dependencies

## 📝 What Was Configured
1. **MSI installer project** with WiX Toolset
2. **Automated build workflow** for x64 and x86
3. **Silent install/uninstall** support
4. **Auto-harvest of application files**
5. **Store submission packaging**

## 📋 Pre-Publishing Checklist

### 1. Package Identity & Metadata
You need to update `Package.appxmanifest` with:
- **Publisher**: Your certificate name (from Partner Center)
- **Package name**: Reserve name in Partner Center first
- **Display name**: "KAIROS AI Chat Assistant" (or your chosen name)
- **Description**: Describe your AI chatbot app
- **Logo images**: Create proper sized icons (44x44, 150x150, etc.)
- **Capabilities**: Review and keep only necessary permissions

### 2. Code Signing Certificate
For actual store submission, you need a valid certificate:

**Option A: Partner Center Certificate (Recommended)**
```xml
<AppxPackageSigningEnabled>True</AppxPackageSigningEnabled>
<PackageCertificateKeyFile>KAIROS_TemporaryKey.pfx</PackageCertificateKeyFile>
```
Download your certificate from Partner Center and add to project.

**Option B: Test Certificate (Development Only)**
```powershell
# Generate test certificate
New-SelfSignedCertificate -Type Custom -Subject "CN=YourPublisher" `
  -KeyUsage DigitalSignature -FriendlyName "KAIROS Dev Cert" `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
```

### 3. Microsoft Store Requirements

#### Age Rating
Your AI chatbot needs an age rating:
- Visit [IARC rating portal](https://www.globalratings.com/)
- Complete questionnaire
- Import rating certificate to Partner Center

#### Privacy Policy
**Required** because you:
- Store user conversations locally (SQLite)
- Download AI models from internet
- Process user text input

Create a privacy policy covering:
- What data is collected (chat messages, locally stored)
- How data is used (AI responses)
- Data storage (local SQLite database)
- Third-party services (model downloads from HuggingFace)

#### Store Assets
Prepare these images:
- **Store Logo**: 300x300 px
- **Screenshots**: At least 1 screenshot (1366x768 or higher)
- **Hero Image** (optional): 1920x1080 px
- **Promotional Images** (optional)

### 4. App Capabilities Audit
Review `Package.appxmanifest` capabilities:
- ? **Internet (Client)**: For model downloads
- ? Remove unused capabilities (Camera, Microphone, etc.)

### 5. Content Compliance

#### AI Model Considerations
- Default model: TinyLlama-1.1B (permissive license ?)
- Ensure model license allows commercial distribution
- Consider content filtering for AI responses
- Add disclaimers about AI-generated content

#### Store Policy Compliance
- ? App must handle offensive content gracefully
- ? Clear that it's an AI assistant
- ? No misleading claims about capabilities

## ?? Publishing Steps

### Step 1: Create App Listing
1. Go to [Partner Center](https://partner.microsoft.com/dashboard)
2. Create new app submission
3. Reserve app name
4. Complete app listing (description, keywords, categories)

### Step 2: Update Package Manifest
Update `Package.appxmanifest`:
```xml
<Identity Name="YourPublisherID.KAIROS" 
          Publisher="CN=YourPublisher" 
          Version="1.0.0.0" />
<Properties>
  <DisplayName>KAIROS - AI Chat Assistant</DisplayName>
  <PublisherDisplayName>Your Company Name</PublisherDisplayName>
  <Logo>Assets\StoreLogo.png</Logo>
</Properties>
```

### Step 3: Build MSIX Package
```powershell
# In Visual Studio
# 1. Right-click project ? Publish ? Create App Packages
# 2. Select "Microsoft Store under a new app name"
# 3. Sign in with Partner Center account
# 4. Select your reserved app name
# 5. Configure architecture (x64, ARM64)
# 6. Build package

# Or via CLI
dotnet publish -c Release -r win-x64 /p:Platform=x64
```

### Step 4: Upload to Partner Center
1. Complete age ratings
2. Upload MSIX package
3. Submit for certification

## ?? Known Issues & Considerations

### Large Model Downloads
- First-run downloads TinyLlama (~637 MB)
- **Consider**: Pre-packaging smaller model or offering download choice
- **Alternative**: Use smaller quantized models (Q2, Q3)

### Offline Functionality
- App requires internet for initial model download
- Works offline after model is downloaded
- Update store description to reflect this

### Performance
- CPU-only inference (set `GpuLayerCount = 0`)
- May be slow on older devices
- Consider performance warnings in app description

### User Data
- All data stored locally (`%LocalAppData%\KAIROS\`)
- Survives app updates
- Consider adding data export/delete features for GDPR compliance

## ?? Security Checklist
- [ ] Code signing certificate obtained
- [ ] Privacy policy published online
- [ ] AI model source verified (HuggingFace URL)
- [ ] No hardcoded secrets or API keys
- [ ] User data encrypted at rest (consider SQLite encryption)
- [ ] HTTPS for all network requests (model downloads)

## ?? Testing Before Submission
1. **Install packaged version**: Run "KAIROS (Package)" profile
2. **Test first-run experience**: Delete `%LocalAppData%\KAIROS` folder
3. **Test model download**: Ensure progress bar works
4. **Test offline**: Disconnect network after model download
5. **Test conversation persistence**: Restart app, verify messages saved
6. **Test on clean machine**: Use fresh Windows VM

## ?? Store Optimization Tips

### App Description
```
KAIROS - Your Personal AI Chat Assistant

Experience the power of AI locally on your Windows device! KAIROS brings 
advanced language models directly to your PC with complete privacy.

FEATURES:
? Local AI - All processing happens on your device
?? Natural Conversations - Chat naturally with the AI
?? Conversation History - Your chats are saved locally
?? Privacy First - No data sent to cloud servers
?? Beautiful Modern UI - Fluent Design with Mica backdrop

REQUIREMENTS:
� Windows 10 version 1809 or higher
� 2GB free disk space (for AI model)
� Internet connection for initial model download
� Offline after first use

Note: First launch downloads a ~637MB AI model. Responses are generated 
locally on your device.
```

### Keywords
- AI Chat
- Chatbot
- AI Assistant
- Local AI
- Privacy AI
- LLM
- Language Model
- Offline AI

### Category
- **Primary**: Productivity
- **Secondary**: AI & Machine Learning (if available)

## ?? Troubleshooting

### Build Errors
If you see APPX1101 errors about duplicate DLLs:
- Verify `RemoveDuplicateLlamaNativeLibs` target in `.csproj`
- Clean and rebuild: `dotnet clean && dotnet build`

### Certification Failures
Common rejection reasons:
- Missing privacy policy link
- Incomplete age rating
- Icons not meeting size requirements
- App crashes on launch (test thoroughly!)

## ?? Support Resources
- [Windows App SDK Documentation](https://learn.microsoft.com/windows/apps/windows-app-sdk/)
- [Microsoft Store Policies](https://learn.microsoft.com/windows/apps/publish/store-policies)
- [Partner Center Help](https://partner.microsoft.com/support)
- [MSIX Packaging](https://learn.microsoft.com/windows/msix/)

## ?? Next Steps
1. [ ] Update Package.appxmanifest with your identity
2. [ ] Create proper app icons and screenshots
3. [ ] Write privacy policy and host online
4. [ ] Get age rating certificate
5. [ ] Register in Partner Center
6. [ ] Build and test MSIX package
7. [ ] Submit for certification

Good luck with your Microsoft Store submission! ??
