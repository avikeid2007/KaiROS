# Security Policy

## Supported Versions

We release patches for security vulnerabilities for the following versions:

| Version | Supported          |
| ------- | ------------------ |
| Latest  | :white_check_mark: |

## Reporting a Vulnerability

We take the security of KaiROS seriously. If you discover a security vulnerability, please report it responsibly.

### How to Report

1. **DO NOT** open a public GitHub issue for security vulnerabilities
2. Use GitHub's Security Advisory feature:
   - Go to the [Security tab](https://github.com/avikeid2007/KaiROS/security)
   - Click "Report a vulnerability"
   - Fill in the details
3. Alternatively, create a private security advisory

### What to Include

When reporting a security issue, please include:

- Description of the vulnerability
- Steps to reproduce the issue
- Potential impact
- Any suggested fixes (if available)
- Your contact information (optional)

### Response Timeline

- **Initial Response**: Within 48 hours
- **Status Update**: Within 7 days
- **Fix Timeline**: Depends on severity
  - Critical: Within 7 days
  - High: Within 30 days
  - Medium: Within 90 days
  - Low: Best effort basis

## Security Considerations

### Local AI Processing

KaiROS runs AI models locally on your device:

- ✅ All processing happens on your PC
- ✅ No data sent to external servers
- ✅ Conversations stored locally in SQLite
- ✅ Models downloaded from trusted sources (Hugging Face)

### Data Storage

- Chat history stored in: `%LocalAppData%\KAIROS\chat.db`
- Models cached in: `%LocalAppData%\KAIROS\Models\`
- All data remains on your device
- No telemetry or analytics collected

### Network Usage

The application only uses network for:

- Initial model downloads from Hugging Face
- After download, works completely offline

### User Responsibilities

Users should:

- Keep Windows and the application updated
- Download models only from official sources
- Be aware that AI responses are generated locally and uncensored
- Not input sensitive information if concerned about local storage

## Security Best Practices

When contributing to KaiROS:

- Never commit secrets, API keys, or credentials
- Validate all user inputs
- Use parameterized queries for database operations
- Keep dependencies updated
- Follow secure coding practices
- Review code for potential vulnerabilities

## Acknowledgments

We appreciate responsible disclosure and will acknowledge security researchers who report valid vulnerabilities.

---

Thank you for helping keep KaiROS and its users safe!
