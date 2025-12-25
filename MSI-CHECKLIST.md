# ?? MSI Packaging - Your Action Checklist

Use this checklist to track your progress toward Microsoft Store submission.

---

## ? Phase 1: Setup (5-10 minutes)

- [ ] **Install WiX Toolset v3.11+**
  - Download from: https://wixtoolset.org/releases/
  - Run installer
  - Restart terminal/IDE

- [ ] **Verify Installation**
  ```powershell
  # Check WiX is in PATH
  where.exe candle.exe
  # Should return: C:\Program Files (x86)\WiX Toolset v3.11\bin\candle.exe
  ```

- [ ] **Review Documentation**
  - Read: [MSI-SETUP-SUMMARY.md](MSI-SETUP-SUMMARY.md)
  - Skim: [KAIROS.Installer/SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md)

---

## ? Phase 2: First Build (10-15 minutes)

- [ ] **Build Application**
  ```powershell
  dotnet build KAIROS.csproj -c Release -p:Platform=x64
  ```

- [ ] **Build MSI Installer**
  ```powershell
  cd KAIROS.Installer
  .\Build-MSI.ps1 -Platform x64 -Configuration Release
  ```

- [ ] **Verify Output**
  - Check file exists: `KAIROS.Installer\bin\x64\Release\KaiROS-AI-Setup.msi`
  - Check file size: Should be 50-200 MB

- [ ] **Run Validation**
  ```powershell
  .\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi" -SkipInstallTest
  ```

---

## ? Phase 3: Local Testing (30-45 minutes)

- [ ] **Silent Install Test**
  ```powershell
  # Run as Administrator
  msiexec /i bin\x64\Release\KaiROS-AI-Setup.msi /quiet /qn /l*v install.log
  ```

- [ ] **Verify Installation**
  - [ ] Check installation folder: `C:\Program Files\Avnish Kumar\KaiROS AI\`
  - [ ] Verify all DLLs present
  - [ ] Check Start Menu shortcut exists
  - [ ] Check Desktop shortcut (if enabled)

- [ ] **Application Testing**
  ```powershell
  # Launch application
  & "C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe"
  ```
  - [ ] Application launches without errors
  - [ ] UI loads correctly
  - [ ] Can select and download a model
  - [ ] Can send chat messages
  - [ ] Chat responses work
  - [ ] Database saves conversations

- [ ] **Silent Uninstall Test**
  ```powershell
  # Run as Administrator
  msiexec /x bin\x64\Release\KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall.log
  ```

- [ ] **Verify Cleanup**
  - [ ] Installation folder removed
  - [ ] Start Menu shortcut removed
  - [ ] Desktop shortcut removed
  - [ ] Registry entries cleaned up
  - [ ] Review uninstall.log for errors

---

## ? Phase 4: Clean Machine Testing (1-2 hours)

**Important**: Test on a machine WITHOUT development tools installed

- [ ] **Prepare Test Environment**
  - [ ] Windows 10 version 1809+ or Windows 11
  - [ ] No Visual Studio installed
  - [ ] No .NET SDK installed (runtime is OK)
  - [ ] Clean, freshly installed or reset Windows

- [ ] **Copy MSI to Test Machine**
  - [ ] Transfer `KaiROS-AI-Setup.msi`
  - [ ] No other files needed

- [ ] **Silent Install on Clean Machine**
  ```powershell
  msiexec /i KaiROS-AI-Setup.msi /quiet /qn /l*v install-clean.log
  ```

- [ ] **Full Application Test**
  - [ ] Application launches
  - [ ] No missing DLL errors
  - [ ] Can download models
  - [ ] Chat functionality works
  - [ ] Database operations work
  - [ ] No crashes

- [ ] **Silent Uninstall on Clean Machine**
  ```powershell
  msiexec /x KaiROS-AI-Setup.msi /quiet /qn /l*v uninstall-clean.log
  ```

- [ ] **Verify Complete Cleanup**
  - [ ] All files removed
  - [ ] All registry entries removed

---

## ? Phase 5: WACK Testing (30-60 minutes)

**Windows App Certification Kit** - Required for Microsoft Store

- [ ] **Install Application on Test Machine**
  ```powershell
  msiexec /i KaiROS-AI-Setup.msi /quiet /qn
  ```

- [ ] **Launch WACK**
  ```powershell
  & "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\appcert.exe"
  ```

- [ ] **Run Certification Tests**
  - [ ] Select "Desktop App"
  - [ ] Browse to: `C:\Program Files\Avnish Kumar\KaiROS AI\KAIROS.exe`
  - [ ] Run all tests
  - [ ] Wait for completion (20-30 minutes)

- [ ] **Review Results**
  - [ ] All tests passed ?
  - [ ] If failures: Review and fix issues
  - [ ] Save WACK report

---

## ? Phase 6: Code Signing (Optional but Recommended)

- [ ] **Obtain Code Signing Certificate**
  - Option A: Purchase from CA (DigiCert, GlobalSign, etc.)
  - Option B: Use company certificate
  - Option C: Skip for initial testing

- [ ] **Sign MSI Package**
  ```powershell
  .\Build-MSI.ps1 -Platform x64 -Configuration Release `
    -Sign -CertificatePath "cert.pfx" -CertificatePassword "password"
  ```

- [ ] **Verify Signature**
  ```powershell
  Get-AuthenticodeSignature bin\x64\Release\KaiROS-AI-Setup.msi
  # Status should be: Valid
  ```

---

## ? Phase 7: Store Listing Preparation (2-3 hours)

- [ ] **Create Store Assets**
  - [ ] App icon (multiple sizes)
  - [ ] Screenshots (1-10 images, at least 1 required)
  - [ ] Hero image (optional)
  - [ ] Promotional images (optional)

- [ ] **Write Store Listing**
  - [ ] App name: "KaiROS AI"
  - [ ] Short description (1-2 sentences)
  - [ ] Full description (detailed, compelling)
  - [ ] Features list
  - [ ] Keywords (search terms)

- [ ] **Legal & Compliance**
  - [ ] Privacy policy (required if collecting data)
  - [ ] Terms of Service
  - [ ] EULA (if different from default)

- [ ] **Support Information**
  - [ ] Support email
  - [ ] Support URL/website
  - [ ] Contact information

---

## ? Phase 8: Partner Center Setup (30-60 minutes)

- [ ] **Create Microsoft Partner Center Account**
  - URL: https://partner.microsoft.com/
  - [ ] Sign up/sign in
  - [ ] Enroll in Windows & Xbox program
  - [ ] Pay enrollment fee (one-time, ~$19-99)
  - [ ] Wait for approval (usually immediate)

- [ ] **Reserve App Name**
  - [ ] Navigate to: Apps and games > New product
  - [ ] Select: Desktop application
  - [ ] Reserve name: "KaiROS AI"
  - [ ] (or choose alternative if taken)

---

## ? Phase 9: Create Submission (1-2 hours)

- [ ] **Start Submission**
  - [ ] Click on your app
  - [ ] Click "Start your submission"

- [ ] **Complete All Sections**

  ### Properties
  - [ ] Category: Productivity (or appropriate)
  - [ ] Subcategory: AI & Machine Learning
  - [ ] System requirements:
    - [ ] OS: Windows 10 version 1809 or higher
    - [ ] Architecture: x64
    - [ ] Memory: 4 GB minimum, 8 GB recommended
    - [ ] Storage: 500 MB

  ### Age Ratings
  - [ ] Complete age rating questionnaire
  - [ ] Typical rating: EVERYONE

  ### Packages
  - [ ] Upload: `KaiROS-AI-Setup.msi`
  - [ ] Platform: x64
  - [ ] Wait for validation
  - [ ] Address any errors

  ### Store Listings
  - [ ] Language: English (United States)
  - [ ] Description: [Your compelling description]
  - [ ] Screenshots: Upload 1-10 images
  - [ ] App icon: Upload required sizes
  - [ ] Keywords: AI, Assistant, Chat, LLM, Local
  - [ ] Privacy policy URL: [Your URL]
  - [ ] Support contact: [Your email]

  ### Pricing and Availability
  - [ ] Markets: Select target countries
  - [ ] Pricing: Free or set price
  - [ ] Visibility: Public or private
  - [ ] Release schedule: Immediate or scheduled

  ### Additional Information (if applicable)
  - [ ] App installer settings
  - [ ] Targeted release
  - [ ] Notes to certification team

- [ ] **Review All Sections**
  - [ ] All required fields completed
  - [ ] Green checkmarks on all sections
  - [ ] No validation errors

---

## ? Phase 10: Submit for Certification (1 minute + wait time)

- [ ] **Final Review**
  - [ ] Double-check all information
  - [ ] Review pricing
  - [ ] Review visibility settings

- [ ] **Submit**
  - [ ] Click "Submit for certification"
  - [ ] Confirm submission

- [ ] **Monitor Submission**
  - [ ] Check Partner Center dashboard daily
  - [ ] Typical review time: 1-3 business days
  - [ ] Respond to any certification failures promptly

---

## ? Phase 11: Post-Submission (Ongoing)

- [ ] **After Approval**
  - [ ] Announce release on social media
  - [ ] Update GitHub README with store link
  - [ ] Monitor user reviews
  - [ ] Respond to feedback

- [ ] **For Updates**
  - [ ] Update version numbers
  - [ ] Rebuild MSI
  - [ ] Test thoroughly
  - [ ] Create new submission in Partner Center
  - [ ] Update release notes

---

## ?? Progress Tracker

**Current Status**: 
- [ ] Phase 1: Setup
- [ ] Phase 2: First Build
- [ ] Phase 3: Local Testing
- [ ] Phase 4: Clean Machine Testing
- [ ] Phase 5: WACK Testing
- [ ] Phase 6: Code Signing (Optional)
- [ ] Phase 7: Store Listing Preparation
- [ ] Phase 8: Partner Center Setup
- [ ] Phase 9: Create Submission
- [ ] Phase 10: Submit for Certification
- [ ] Phase 11: Post-Submission

**Target Completion**: _______________

**Notes**:
```
[Add your notes here]
```

---

## ?? Help & Resources

### If You Get Stuck

**Build Issues**: [KAIROS.Installer/README.md](KAIROS.Installer/README.md)

**Certification Issues**: [KAIROS.Installer/SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md#troubleshooting)

**Command Reference**: [KAIROS.Installer/QUICK-REFERENCE.md](KAIROS.Installer/QUICK-REFERENCE.md)

**GitHub Issues**: https://github.com/avikeid2007/KaiROS/issues

---

## ?? Completion

When all checkboxes are complete, you will have:
- ? A production-ready MSI installer
- ? Passed all Microsoft Store certification requirements
- ? Published app on Microsoft Store
- ? Happy users! ??

**Good luck with your submission!** ??
