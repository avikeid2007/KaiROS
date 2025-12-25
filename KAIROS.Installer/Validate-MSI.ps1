# MSI Validation Script for Microsoft Store Submission
# This script performs automated validation tests required for MS Store certification

param(
    [Parameter(Mandatory=$true)]
    [string]$MSIPath,
    
    [Parameter(Mandatory=$false)]
    [string]$ExtractPath = "$env:TEMP\KaiROS-MSI-Validation",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipInstallTest,
    
    [Parameter(Mandatory=$false)]
    [switch]$CleanupAfterTest
)

$ErrorActionPreference = "Continue"
$ValidationErrors = @()
$ValidationWarnings = @()
$ValidationSuccess = @()

function Write-ValidationResult {
    param(
        [string]$Message,
        [ValidateSet('Success', 'Warning', 'Error')]
        [string]$Type = 'Success'
    )
    
    switch ($Type) {
        'Success' {
            Write-Host "? $Message" -ForegroundColor Green
            $script:ValidationSuccess += $Message
        }
        'Warning' {
            Write-Host "? $Message" -ForegroundColor Yellow
            $script:ValidationWarnings += $Message
        }
        'Error' {
            Write-Host "? $Message" -ForegroundColor Red
            $script:ValidationErrors += $Message
        }
    }
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "MSI Validation for Microsoft Store" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Validate MSI exists
if (-not (Test-Path $MSIPath)) {
    Write-ValidationResult "MSI file not found: $MSIPath" -Type Error
    exit 1
}

Write-ValidationResult "MSI file found: $MSIPath"
Write-Host ""

# Test 1: File Size Validation
Write-Host "Test 1: File Size Validation" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow
$msiSize = (Get-Item $MSIPath).Length / 1MB
Write-Host "MSI Size: $([math]::Round($msiSize, 2)) MB" -ForegroundColor Cyan

if ($msiSize -gt 500) {
    Write-ValidationResult "MSI size exceeds 500 MB - may cause store submission issues" -Type Warning
} else {
    Write-ValidationResult "MSI size is acceptable"
}
Write-Host ""

# Test 2: MSI Properties Validation
Write-Host "Test 2: MSI Properties Validation" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

try {
    $windowsInstaller = New-Object -ComObject WindowsInstaller.Installer
    $database = $windowsInstaller.GetType().InvokeMember("OpenDatabase", "InvokeMethod", $null, $windowsInstaller, @($MSIPath, 0))
    
    # Check required properties
    $requiredProperties = @{
        'ProductName' = $null
        'ProductVersion' = $null
        'Manufacturer' = $null
        'ProductCode' = $null
        'UpgradeCode' = $null
    }
    
    $query = "SELECT * FROM Property"
    $view = $database.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $database, ($query))
    $view.GetType().InvokeMember("Execute", "InvokeMethod", $null, $view, $null)
    
    $record = $view.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $view, $null)
    while ($record -ne $null) {
        $property = $record.GetType().InvokeMember("StringData", "GetProperty", $null, $record, 1)
        $value = $record.GetType().InvokeMember("StringData", "GetProperty", $null, $record, 2)
        
        if ($requiredProperties.ContainsKey($property)) {
            $requiredProperties[$property] = $value
            Write-Host "  $property : $value" -ForegroundColor Cyan
        }
        
        $record = $view.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $view, $null)
    }
    
    # Validate all required properties are present
    foreach ($prop in $requiredProperties.Keys) {
        if ([string]::IsNullOrEmpty($requiredProperties[$prop])) {
            Write-ValidationResult "Missing required property: $prop" -Type Error
        } else {
            Write-ValidationResult "Property $prop is present"
        }
    }
    
    # Cleanup COM objects
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($view) | Out-Null
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($database) | Out-Null
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($windowsInstaller) | Out-Null
    [System.GC]::Collect()
    [System.GC]::WaitForPendingFinalizers()
    
} catch {
    Write-ValidationResult "Failed to read MSI properties: $($_.Exception.Message)" -Type Error
}
Write-Host ""

# Test 3: Extract MSI and validate contents
Write-Host "Test 3: File Extraction Validation" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

if (Test-Path $ExtractPath) {
    Remove-Item $ExtractPath -Recurse -Force
}
New-Item -ItemType Directory -Path $ExtractPath -Force | Out-Null

Write-Host "Extracting MSI to: $ExtractPath" -ForegroundColor Cyan
$extractLogPath = Join-Path $ExtractPath "extract.log"

$extractArgs = @(
    "/a",
    "`"$MSIPath`"",
    "/qn",
    "TARGETDIR=`"$ExtractPath`"",
    "/l*v",
    "`"$extractLogPath`""
)

$process = Start-Process "msiexec.exe" -ArgumentList $extractArgs -Wait -PassThru -NoNewWindow

if ($process.ExitCode -eq 0) {
    Write-ValidationResult "MSI extraction successful"
    
    # Check for required files
    $requiredFiles = @(
        "KAIROS.exe",
        "llama.dll",
        "ggml.dll",
        "ggml-base.dll",
        "ggml-cpu.dll"
    )
    
    $extractedProgramFiles = Get-ChildItem -Path $ExtractPath -Recurse -File
    
    foreach ($file in $requiredFiles) {
        $found = $extractedProgramFiles | Where-Object { $_.Name -eq $file }
        if ($found) {
            Write-ValidationResult "Required file found: $file"
        } else {
            Write-ValidationResult "Required file missing: $file" -Type Error
        }
    }
    
} else {
    Write-ValidationResult "MSI extraction failed with exit code: $($process.ExitCode)" -Type Error
}
Write-Host ""

# Test 4: Silent Installation Test (requires admin)
if (-not $SkipInstallTest) {
    Write-Host "Test 4: Silent Installation Test" -ForegroundColor Yellow
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    
    $isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    
    if (-not $isAdmin) {
        Write-ValidationResult "Skipping installation test - requires administrator privileges" -Type Warning
    } else {
        Write-Host "Testing silent installation..." -ForegroundColor Cyan
        $installLogPath = Join-Path $ExtractPath "install-test.log"
        
        $installArgs = @(
            "/i",
            "`"$MSIPath`"",
            "/quiet",
            "/qn",
            "/l*v",
            "`"$installLogPath`""
        )
        
        $installProcess = Start-Process "msiexec.exe" -ArgumentList $installArgs -Wait -PassThru -NoNewWindow
        
        if ($installProcess.ExitCode -eq 0) {
            Write-ValidationResult "Silent installation succeeded"
            
            # Test uninstallation
            Write-Host "Testing silent uninstallation..." -ForegroundColor Cyan
            $uninstallLogPath = Join-Path $ExtractPath "uninstall-test.log"
            
            $uninstallArgs = @(
                "/x",
                "`"$MSIPath`"",
                "/quiet",
                "/qn",
                "/l*v",
                "`"$uninstallLogPath`""
            )
            
            $uninstallProcess = Start-Process "msiexec.exe" -ArgumentList $uninstallArgs -Wait -PassThru -NoNewWindow
            
            if ($uninstallProcess.ExitCode -eq 0) {
                Write-ValidationResult "Silent uninstallation succeeded"
            } else {
                Write-ValidationResult "Silent uninstallation failed with exit code: $($uninstallProcess.ExitCode)" -Type Error
            }
            
        } else {
            Write-ValidationResult "Silent installation failed with exit code: $($installProcess.ExitCode)" -Type Error
        }
    }
} else {
    Write-Host "Test 4: Skipped (SkipInstallTest flag set)" -ForegroundColor Yellow
}
Write-Host ""

# Test 5: Code Signing Validation
Write-Host "Test 5: Code Signing Validation" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

try {
    $signature = Get-AuthenticodeSignature -FilePath $MSIPath
    
    if ($signature.Status -eq 'Valid') {
        Write-ValidationResult "MSI is digitally signed"
        Write-Host "  Signer: $($signature.SignerCertificate.Subject)" -ForegroundColor Cyan
        Write-Host "  Issued by: $($signature.SignerCertificate.Issuer)" -ForegroundColor Cyan
    } elseif ($signature.Status -eq 'NotSigned') {
        Write-ValidationResult "MSI is not digitally signed - signing recommended for production" -Type Warning
    } else {
        Write-ValidationResult "MSI signature is invalid: $($signature.Status)" -Type Error
    }
} catch {
    Write-ValidationResult "Failed to check digital signature: $($_.Exception.Message)" -Type Warning
}
Write-Host ""

# Generate Report
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "VALIDATION SUMMARY" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Successes: $($ValidationSuccess.Count)" -ForegroundColor Green
Write-Host "Warnings: $($ValidationWarnings.Count)" -ForegroundColor Yellow
Write-Host "Errors: $($ValidationErrors.Count)" -ForegroundColor Red
Write-Host ""

if ($ValidationErrors.Count -gt 0) {
    Write-Host "ERRORS FOUND:" -ForegroundColor Red
    foreach ($error in $ValidationErrors) {
        Write-Host "  • $error" -ForegroundColor Red
    }
    Write-Host ""
}

if ($ValidationWarnings.Count -gt 0) {
    Write-Host "WARNINGS:" -ForegroundColor Yellow
    foreach ($warning in $ValidationWarnings) {
        Write-Host "  • $warning" -ForegroundColor Yellow
    }
    Write-Host ""
}

# Generate detailed report
$reportPath = Join-Path $ExtractPath "ValidationReport.txt"
$report = @"
MSI VALIDATION REPORT
Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
MSI Path: $MSIPath
========================================

SUCCESSES ($($ValidationSuccess.Count)):
$(if ($ValidationSuccess.Count -gt 0) { $ValidationSuccess | ForEach-Object { "  ? $_" } | Out-String } else { "  None" })

WARNINGS ($($ValidationWarnings.Count)):
$(if ($ValidationWarnings.Count -gt 0) { $ValidationWarnings | ForEach-Object { "  ? $_" } | Out-String } else { "  None" })

ERRORS ($($ValidationErrors.Count)):
$(if ($ValidationErrors.Count -gt 0) { $ValidationErrors | ForEach-Object { "  ? $_" } | Out-String } else { "  None" })

========================================
MICROSOFT STORE CERTIFICATION CHECKLIST:
========================================

Manual Steps Required:
? Run Windows App Certification Kit (WACK)
? Test on clean Windows 10 1809+ machine
? Verify application launches after installation
? Verify all features work correctly
? Test upgrade from previous version
? Review privacy policy and data collection
? Prepare store listing assets (screenshots, description)
? Submit to Microsoft Partner Center

References:
- App Certification Process: https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/app-certification-process
- Manual Package Validation: https://learn.microsoft.com/en-us/windows/apps/publish/publish-your-app/msi/manual-package-validation

========================================
"@

$report | Out-File -FilePath $reportPath -Encoding UTF8
Write-Host "Detailed report saved to: $reportPath" -ForegroundColor Cyan
Write-Host ""

# Cleanup
if ($CleanupAfterTest -and (Test-Path $ExtractPath)) {
    Write-Host "Cleaning up temporary files..." -ForegroundColor Yellow
    Remove-Item $ExtractPath -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "Cleanup completed" -ForegroundColor Green
}

# Exit code
if ($ValidationErrors.Count -gt 0) {
    Write-Host "VALIDATION FAILED - Please address errors before submission" -ForegroundColor Red
    exit 1
} elseif ($ValidationWarnings.Count -gt 0) {
    Write-Host "VALIDATION COMPLETED WITH WARNINGS - Review before submission" -ForegroundColor Yellow
    exit 0
} else {
    Write-Host "VALIDATION PASSED - Ready for Microsoft Store submission" -ForegroundColor Green
    exit 0
}
