# Push KAIROS to GitHub - Complete Guide

## 🚀 Quick Start (5 Steps)

### Step 1: Initialize Git Repository
```powershell
# Navigate to your project directory
cd C:\Users\%USERNAME%\source\repos\KaiROS

# Initialize git
git init

# Check status
git status
```

### Step 2: Add Files to Git
```powershell
# Add all files (respects .gitignore)
git add .

# Verify what will be committed
git status
```

### Step 3: Create Initial Commit
```powershell
# Commit with message
git commit -m "Initial commit: KAIROS AI Chat Assistant

- Beautiful WinUI 3 chat interface
- 6 curated LLM models (Phi-3, Mistral, LLaMA, Gemma)
- Local AI processing with LLamaSharp
- SQLite conversation history
- Model selection dialog
- Download progress tracking
- Streaming responses
- Full privacy (100% local)"
```

### Step 4: Create GitHub Repository

#### Option A: Using GitHub CLI (Recommended)
```powershell
# Install GitHub CLI if not installed
winget install --id GitHub.cli

# Login to GitHub
gh auth login

# Create repository and push
gh repo create KaiROS --public --source=. --remote=origin --push

# Or create as private
gh repo create KaiROS --private --source=. --remote=origin --push
```

#### Option B: Using GitHub Website
1. Go to [GitHub](https://github.com)
2. Click **"+" ? New repository**
3. Repository name: `KaiROS`
4. Description: `AI Chat Assistant - Privacy-focused local LLM chatbot for Windows`
5. Choose **Public** or **Private**
6. **DON'T** initialize with README (we have one)
7. Click **Create repository**

### Step 5: Push to GitHub
```powershell
# Add remote (replace <username> with your GitHub username)
git remote add origin https://github.com/<username>/KaiROS.git

# Verify remote
git remote -v

# Push to GitHub
git push -u origin main

# If it says 'master' instead of 'main':
git branch -M main
git push -u origin main
```

---

## 📖 Detailed Instructions

### Prerequisites

1. **Git Installed**
   ```powershell
   # Check if git is installed
   git --version
   
   # If not, install
   winget install --id Git.Git -e --source winget
   ```

2. **GitHub Account**
   - Sign up at [github.com](https://github.com)
   - Verify your email

3. **Git Configuration**
   ```powershell
   # Set your name
   git config --global user.name "Your Name"
   
   # Set your email
   git config --global user.email "your.email@example.com"
   
   # Verify
   git config --list
   ```

---

## 🔐 Authentication Options

### Option 1: Personal Access Token (Recommended)

1. **Create Token**
   - Go to GitHub ? Settings ? Developer settings ? Personal access tokens ? Tokens (classic)
   - Click "Generate new token (classic)"
   - Name: `KAIROS Development`
   - Expiration: `90 days` (or your preference)
   - Select scopes:
     - ? `repo` (all)
     - ? `workflow`
   - Click "Generate token"
   - **Copy the token immediately** (you won't see it again!)

2. **Use Token**
   ```powershell
   # When pushing, use token as password
   git push -u origin main
   # Username: your-github-username
   # Password: paste-your-token-here
   ```

3. **Cache Credentials** (Optional)
   ```powershell
   # Windows Credential Manager
   git config --global credential.helper wincred
   ```

### Option 2: SSH Key

1. **Generate SSH Key**
   ```powershell
   # Generate new SSH key
   ssh-keygen -t ed25519 -C "your.email@example.com"
   
   # Press Enter to accept default location
   # Enter passphrase (optional but recommended)
   ```

2. **Add to SSH Agent**
   ```powershell
   # Start SSH agent
   Start-Service ssh-agent
   
   # Add key
   ssh-add ~\.ssh\id_ed25519
   ```

3. **Add to GitHub**
   ```powershell
   # Copy public key
   Get-Content ~\.ssh\id_ed25519.pub | clip
   ```
   - Go to GitHub ? Settings ? SSH and GPG keys
   - Click "New SSH key"
   - Title: `KAIROS Development PC`
   - Paste the key
   - Click "Add SSH key"

4. **Use SSH URL**
   ```powershell
   # Add remote with SSH
   git remote add origin git@github.com:<username>/KaiROS.git
   ```

---

## 📦 What Gets Pushed?

### ? Included Files
- Source code (`.cs`, `.xaml`, `.csproj`)
- Documentation (`.md` files)
- Assets (images, icons)
- Configuration files
- `.gitignore`
- `LICENSE`
- `README.md`

### ? Excluded Files (via .gitignore)
- Build output (`bin/`, `obj/`)
- User settings (`.vs/`, `*.user`)
- NuGet packages
- Database files (`*.db`)
- Downloaded models (`*.gguf`)
- Temporary files

---

## ✅ Verification

After pushing, verify your repository:

1. **Check GitHub Web**
   - Go to `https://github.com/<username>/KaiROS`
   - You should see all files and README

2. **Clone Test** (Optional)
   ```powershell
   # Clone in different directory
   cd C:\Temp
   git clone https://github.com/<username>/KaiROS.git
   cd KaiROS
   
   # Build to verify
   dotnet build
   ```

---

## 🔄 Future Updates

### Making Changes and Pushing
```powershell
# Make your changes in code

# Check what changed
git status

# Stage changes
git add .

# Commit with descriptive message
git commit -m "feat: Add GPU acceleration support"

# Push to GitHub
git push

# Or create new branch for feature
git checkout -b feature/new-feature
git add .
git commit -m "feat: Add new feature"
git push -u origin feature/new-feature
```

### Commit Message Convention
```
type: brief description

- Bullet point details
- More details

Types:
- feat: New feature
- fix: Bug fix
- docs: Documentation
- style: Formatting
- refactor: Code restructuring
- test: Tests
- chore: Maintenance
```

---

## 🏷️ Releases

### Create a Release
1. **Tag a version**
   ```powershell
   git tag -a v1.0.0 -m "Release version 1.0.0

   Features:
   - 6 AI models
   - Beautiful UI
   - Local processing
   - SQLite storage"
   
   git push origin v1.0.0
   ```

2. **GitHub Release**
   - Go to GitHub repository
   - Click "Releases" ? "Create a new release"
   - Choose tag: `v1.0.0`
   - Title: `KAIROS v1.0.0 - Initial Release`
   - Describe features
   - Attach `.msix` if you have it
   - Click "Publish release"

---

## ⚙️ Repository Settings

### Recommended Settings

1. **Description**
   ```
   AI Chat Assistant - Privacy-focused local LLM chatbot for Windows with beautiful UI
   ```

2. **Topics** (tags)
   ```
   ai, chatbot, llm, windows, winui3, llamasharp, privacy, local-ai, 
   chat-assistant, csharp, dotnet, machine-learning
   ```

3. **About Section**
   - Website: (your website if any)
   - Topics: (add tags above)

4. **Features to Enable**
   - ? Issues
   - ? Projects (if you want project board)
   - ? Wiki (optional)
   - ? Discussions (recommended)

---

## 🔒 Security

### Add Security Policy
Create `SECURITY.md`:
```markdown
# Security Policy

## Reporting a Vulnerability

If you discover a security vulnerability, please report it through GitHub's security advisory feature or create a private security advisory.

Please do not open public issues for security vulnerabilities.

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
```

### Add Code of Conduct
```powershell
# GitHub can auto-generate this
# Go to repository ? Settings ? Community Standards
# Click "Add" next to Code of Conduct
```

---

## 🔧 GitHub Actions (CI/CD)

### Add Build Workflow
Create `.github/workflows/build.yml`:
```yaml
name: Build and Test

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

---

## ? Checklist

Before pushing to GitHub:

- [ ] `.gitignore` created
- [ ] `README.md` updated with your info
- [ ] `LICENSE` file present
- [ ] Personal info removed from code
- [ ] Secrets/tokens removed
- [ ] Build succeeds locally
- [ ] Update email in README
- [ ] Update GitHub username in URLs

---

## 🎉 Success!

Your repository is now on GitHub! 

### Next Steps:
1. ? Star your own repository
2. 📸 Update README with screenshots
3. 🏷️ Create first release
4. 📢 Share with community
5. 🤝 Accept contributions

---

## ❓ Troubleshooting

### Problem: "fatal: remote origin already exists"
```powershell
# Remove existing remote
git remote remove origin

# Add again
git remote add origin https://github.com/<username>/KaiROS.git
```

### Problem: "refusing to merge unrelated histories"
```powershell
# If GitHub repo has files
git pull origin main --allow-unrelated-histories
```

### Problem: Large files rejected
```powershell
# Check file sizes
git ls-files -z | xargs -0 ls -lh | sort -k5 -h

# Remove large files from git history
git rm --cached path/to/large/file
git commit -m "Remove large file"
```

### Problem: Authentication failed
```powershell
# Use personal access token instead of password
# Or set up SSH keys (see Authentication section)
```

---

## 💬 Need Help?

- GitHub Docs: https://docs.github.com/
- Git Docs: https://git-scm.com/doc
- Git Cheat Sheet: https://education.github.com/git-cheat-sheet-education.pdf

---

**Ready to push? Follow Step 1-5 above!** 🚀
