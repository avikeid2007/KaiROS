# Privacy Policy

**Last Updated: December 20, 2024**

## Overview

KaiROS ("we", "our", or "the application") is a Windows desktop application committed to protecting your privacy. This Privacy Policy explains how KaiROS handles your data and information.

## Our Privacy Promise

**KaiROS is designed with privacy as a core principle.** All AI processing happens locally on your device. We do not collect, store, transmit, or have access to your conversations or personal data.

## Data Collection

### What We DON'T Collect

- ❌ **No personal information** - We don't collect names, emails, or any identifying information
- ❌ **No chat history** - Your conversations are never sent to us or any third party
- ❌ **No usage analytics** - We don't track how you use the application
- ❌ **No telemetry** - No diagnostic data is collected or transmitted
- ❌ **No cookies** - As a desktop application, we don't use cookies or tracking technologies
- ❌ **No cloud storage** - Your data never leaves your device

### What Data Stays Local

KaiROS stores the following data **only on your local Windows device**:

- **Conversation History**: Stored in a SQLite database at `%LocalAppData%\KAIROS\chat.db` (Windows path)
- **AI Models**: Downloaded models cached at `%LocalAppData%\KAIROS\Models\` (Windows path)
- **Application Settings**: User preferences and configuration

**You have full control** over this data and can delete it at any time by removing these files or uninstalling the application.

## Network Usage

KaiROS uses network connectivity only for:

1. **Initial Model Download**: When you first select an AI model, it is downloaded from [Hugging Face](https://huggingface.co/), a trusted third-party model repository
2. **Model Updates**: If you choose to download additional or updated models

After the initial download, **KaiROS works completely offline**. No network connection is required for chat functionality.

### Third-Party Services

The only third-party service KaiROS interacts with is:

- **Hugging Face** ([huggingface.co](https://huggingface.co/)): Used exclusively for downloading AI models
  - Hugging Face may have their own privacy policy and terms of service
  - Model downloads are standard HTTPS file transfers
  - No personal data is shared during downloads

## How Your Data is Protected

### Local Processing

- **100% Local AI**: All AI inference happens on your Windows PC using locally stored models
- **No Cloud Servers**: We don't operate any servers that could access your data
- **Offline Capable**: After model download, the application runs entirely offline

### Data Storage

- **Local SQLite Database**: Conversations stored in a local database on your device
- **User-Controlled**: You can backup, export, or delete your data at any time
- **No Synchronization**: Data is never synced to cloud services

### Open Source

- **Transparent**: Our source code is publicly available on [GitHub](https://github.com/avikeid2007/KaiROS)
- **Auditable**: Anyone can review our code to verify our privacy claims
- **Community-Driven**: Security researchers can identify and report issues

## Your Rights and Control

You have complete control over your data:

### Right to Access
- All your data is stored locally on your device
- You can access your conversation database directly

### Right to Delete
- Uninstalling KaiROS removes all application data
- You can manually delete the database at `%LocalAppData%\KAIROS\`
- Individual conversations can be deleted within the application

### Right to Export
- Export conversations to Markdown format
- Direct access to SQLite database for custom exports

### Right to Opt-Out
- No data collection means nothing to opt out of
- You maintain complete privacy by design

## Children's Privacy

KaiROS does not knowingly collect any information from anyone, including children under the age of 13. Since all processing is local and no data is transmitted, the application can be used safely by users of all ages under appropriate supervision.

## Data Retention

- **Conversation Data**: Retained locally until you delete it
- **AI Models**: Cached locally until you delete them
- **No Server-Side Retention**: We don't retain any data because we don't collect any data

## Changes to This Privacy Policy

We may update this Privacy Policy from time to time. Changes will be reflected in the "Last Updated" date at the top of this document. Continued use of KaiROS after changes constitutes acceptance of the updated policy.

## California Privacy Rights (CCPA)

California residents have specific rights under the California Consumer Privacy Act (CCPA):

- **Right to Know**: We don't collect personal information
- **Right to Delete**: All data is local and can be deleted by you
- **Right to Opt-Out of Sale**: We don't sell any data (we don't collect any)
- **Non-Discrimination**: Not applicable as we don't collect data

## European Privacy Rights (GDPR)

For users in the European Union:

- **Data Controller**: Not applicable - all data remains on your device
- **Legal Basis**: Not applicable - no data processing occurs outside your device
- **Data Protection Officer**: Not required for local-only applications
- **Right to Portability**: Export your conversations at any time
- **Right to Erasure**: Delete your data at any time

## Security

While we don't collect data, we take security seriously:

- **Local Storage Security**: Data files are protected by your Windows user account permissions and device security (note: SQLite database files are not encrypted by default)
- **No Network Exposure**: Conversations never transmitted over networks
- **Regular Updates**: Security patches provided through application updates
- **Secure Dependencies**: We use well-maintained, security-audited libraries

For security vulnerabilities, please see our [Security Policy](SECURITY.md).

## Contact Us

If you have questions about this Privacy Policy:

- **GitHub Issues**: [Open an issue](https://github.com/avikeid2007/KaiROS/issues)
- **GitHub Discussions**: [Start a discussion](https://github.com/avikeid2007/KaiROS/discussions)
- **Security Issues**: See our [Security Policy](SECURITY.md)

## Consent

By using KaiROS, you consent to this Privacy Policy. Since we don't collect any data, your privacy is protected by default.

---

## Summary

**TL;DR**: KaiROS is designed for maximum privacy:

✅ **All AI processing happens locally on your device**  
✅ **No data collection or transmission to external servers**  
✅ **Conversations stored only on your PC**  
✅ **No analytics, telemetry, or tracking**  
✅ **Open source and auditable**  
✅ **Works offline after initial model download**  

**Your conversations are yours alone. We can't see them, we don't want to see them, and our architecture ensures we never will.**

---

<div align="center">

**Privacy by Design | Security by Default**

[⬆️ Back to Top](#privacy-policy)

</div>
