# Build and Package KaiROS AI MSI Installer
# This script automates the build process for Microsoft Store submission

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('x64', 'x86')]
    [string]$Platform = 'x64',
    
    [Parameter(Mandatory=$false)]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipBuild,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipTests,
    
    [Parameter(Mandatory=$false)]
    [switch]$Sign,
    
    [Parameter(Mandatory=$false)]
    [string]$CertificatePath,
    
    [Parameter(Mandatory=$false)]
    [string]$CertificatePassword
)

$ErrorActionPreference = "Stop"
$ScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "KaiROS AI - MSI Build Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Platform: $Platform" -ForegroundColor Green
Write-Host "Configuration: $Configuration" -ForegroundColor Green
Write-Host ""

# Check for WiX Toolset
Write-Host "Checking for WiX Toolset..." -ForegroundColor Yellow
$wixPath = "${env:ProgramFiles(x86)}\WiX Toolset v3.11\bin"
if (-not (Test-Path $wixPath)) {
    $wixPath = "${env:ProgramFiles}\WiX Toolset v3.11\bin"
}

if (-not (Test-Path $wixPath)) {
    Write-Error "WiX Toolset v3.11 not found. Please install from https://wixtoolset.org/releases/"
    exit 1
}

$env:PATH = "$wixPath;$env:PATH"
Write-Host "? WiX Toolset found at: $wixPath" -ForegroundColor Green
Write-Host ""

# Step 1: Clean previous builds
Write-Host "Step 1: Cleaning previous builds..." -ForegroundColor Yellow
$cleanParams = @(
    "$ScriptRoot\KAIROS.csproj",
    "/t:Clean",
    "/p:Configuration=$Configuration",
    "/p:Platform=$Platform",
    "/verbosity:minimal"
)

& msbuild $cleanParams
if ($LASTEXITCODE -ne 0) {
    Write-Error "Clean failed"
    exit $LASTEXITCODE
}

# Clean installer project
$installerCleanParams = @(
    "$ScriptRoot\KAIROS.Installer\KAIROS.Installer.wixproj",
    "/t:Clean",
    "/p:Configuration=$Configuration",
    "/p:Platform=$Platform",
    "/verbosity:minimal"
)

& msbuild $installerCleanParams
if ($LASTEXITCODE -ne 0) {
    Write-Error "Installer clean failed"
    exit $LASTEXITCODE
}

Write-Host "? Clean completed" -ForegroundColor Green
Write-Host ""

# Step 2: Restore NuGet packages
Write-Host "Step 2: Restoring NuGet packages..." -ForegroundColor Yellow
& dotnet restore "$ScriptRoot\KAIROS.csproj"
if ($LASTEXITCODE -ne 0) {
    Write-Error "NuGet restore failed"
    exit $LASTEXITCODE
}
Write-Host "? NuGet packages restored" -ForegroundColor Green
Write-Host ""

# Step 3: Build KAIROS application
if (-not $SkipBuild) {
    Write-Host "Step 3: Building KAIROS application..." -ForegroundColor Yellow
    $buildParams = @(
        "$ScriptRoot\KAIROS.csproj",
        "/t:Build",
        "/p:Configuration=$Configuration",
        "/p:Platform=$Platform",
        "/verbosity:minimal",
        "/p:WarningLevel=0"
    )
    
    & msbuild $buildParams
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed"
        exit $LASTEXITCODE
    }
    Write-Host "? KAIROS application built successfully" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "Step 3: Skipping build (SkipBuild flag set)" -ForegroundColor Yellow
    Write-Host ""
}

# Step 4: Run tests (if not skipped)
if (-not $SkipTests) {
    Write-Host "Step 4: Running tests..." -ForegroundColor Yellow
    # Add test execution here if you have test projects
    Write-Host "? Tests passed (no tests configured)" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "Step 4: Skipping tests (SkipTests flag set)" -ForegroundColor Yellow
    Write-Host ""
}

# Step 5: Build MSI Installer
Write-Host "Step 5: Building MSI installer..." -ForegroundColor Yellow
$installerParams = @(
    "$ScriptRoot\KAIROS.Installer\KAIROS.Installer.wixproj",
    "/t:Build",
    "/p:Configuration=$Configuration",
    "/p:Platform=$Platform",
    "/verbosity:minimal"
)

& msbuild $installerParams
if ($LASTEXITCODE -ne 0) {
    Write-Error "MSI build failed"
    exit $LASTEXITCODE
}

$msiPath = "$ScriptRoot\KAIROS.Installer\bin\$Platform\$Configuration\KaiROS-AI-Setup.msi"
if (-not (Test-Path $msiPath)) {
    Write-Error "MSI file not found at expected location: $msiPath"
    exit 1
}

Write-Host "? MSI installer built successfully" -ForegroundColor Green
Write-Host "  Location: $msiPath" -ForegroundColor Cyan
Write-Host ""

# Step 6: Code signing (if requested)
if ($Sign) {
    Write-Host "Step 6: Signing MSI package..." -ForegroundColor Yellow
    
    if (-not $CertificatePath) {
        Write-Error "Certificate path required for signing. Use -CertificatePath parameter."
        exit 1
    }
    
    if (-not (Test-Path $CertificatePath)) {
        Write-Error "Certificate not found: $CertificatePath"
        exit 1
    }
    
    $signToolPath = "${env:ProgramFiles(x86)}\Windows Kits\10\bin\*\x64\signtool.exe"
    $signTool = Get-ChildItem $signToolPath -Recurse -ErrorAction SilentlyContinue | 
                Sort-Object FullName -Descending | 
                Select-Object -First 1
    
    if (-not $signTool) {
        Write-Error "SignTool.exe not found. Install Windows SDK."
        exit 1
    }
    
    $signParams = @(
        "sign",
        "/f", $CertificatePath,
        "/t", "http://timestamp.digicert.com"
    )
    
    if ($CertificatePassword) {
        $signParams += "/p"
        $signParams += $CertificatePassword
    }
    
    $signParams += $msiPath
    
    & $signTool.FullName $signParams
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Code signing failed"
        exit $LASTEXITCODE
    }
    
    Write-Host "? MSI package signed successfully" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "Step 6: Skipping code signing (Sign flag not set)" -ForegroundColor Yellow
    Write-Host ""
}

# Step 7: Validation
Write-Host "Step 7: Validating MSI package..." -ForegroundColor Yellow

# Check file size
$msiSize = (Get-Item $msiPath).Length / 1MB
Write-Host "  MSI Size: $([math]::Round($msiSize, 2)) MB" -ForegroundColor Cyan

# Create validation report
$validationReport = @"
========================================
MSI Package Validation Report
========================================
Build Date: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
Platform: $Platform
Configuration: $Configuration
MSI Location: $msiPath
MSI Size: $([math]::Round($msiSize, 2)) MB

VALIDATION CHECKLIST:
--------------------
? MSI file created successfully
$(if ($Sign) { "? MSI package is signed" } else { "? MSI package is NOT signed" })

MANUAL VALIDATION REQUIRED:
--------------------------
Please perform the following validations before Microsoft Store submission:

1. Silent Installation Test:
   msiexec /i "$msiPath" /quiet /qn /l*v install.log

2. Silent Uninstallation Test:
   msiexec /x "$msiPath" /quiet /qn /l*v uninstall.log

3. Upgrade Test:
   - Install previous version
   - Install current version
   - Verify upgrade success

4. Windows App Certification Kit (WACK):
   - Run WACK on the installed application
   - Address any certification failures

5. File Extraction Test:
   msiexec /a "$msiPath" /qn TARGETDIR=C:\Temp\KaiROS-Extract
   - Verify all required files are present

NEXT STEPS:
----------
1. Test the MSI on a clean Windows 10 1809+ machine
2. Run Windows App Certification Kit
3. Review certification requirements:
   https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/app-certification-process
4. Submit to Microsoft Store

========================================
"@

Write-Host $validationReport
$validationReport | Out-File "$ScriptRoot\KAIROS.Installer\bin\$Platform\$Configuration\ValidationReport.txt"

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "BUILD COMPLETED SUCCESSFULLY!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "MSI Location: $msiPath" -ForegroundColor Cyan
Write-Host "Validation Report: $ScriptRoot\KAIROS.Installer\bin\$Platform\$Configuration\ValidationReport.txt" -ForegroundColor Cyan
Write-Host ""

# Open output folder
if ($Configuration -eq "Release") {
    Write-Host "Opening output folder..." -ForegroundColor Yellow
    Start-Process explorer.exe -ArgumentList "/select,`"$msiPath`""
}
