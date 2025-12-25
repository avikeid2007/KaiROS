# KAIROS - AI Chat Assistant

<div align="center">

![KAIROS Logo](Assets/Square150x150Logo.scale-200.png)

**A beautiful, privacy-focused AI chat assistant for Windows**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![WinUI 3](https://img.shields.io/badge/WinUI-3-0078D4?logo=windows)](https://microsoft.github.io/microsoft-ui-xaml/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

[Features](#-features) • [Installation](#-installation) • [Screenshots](#-screenshots) • [Build](#-building) • [Contributing](#-contributing)

</div>

---

## ✨ Features

### 🔒 **Local AI Processing**
- Run powerful LLM models **100% locally** on your Windows PC
- Complete privacy - no data sent to cloud servers
- Works offline after initial model download
- Powered by [LLamaSharp](https://github.com/SciSharp/LLamaSharp)

### 💬 **Beautiful Chat Interface**
- Modern Fluent Design with Mica backdrop
- Smooth text streaming as AI generates responses
- Message history with chat bubbles
- Responsive and intuitive UI

### 🤖 **6 Curated AI Models**
Choose from professionally selected models:

| Model | Size | Best For | Min RAM |
|-------|------|----------|---------|
| **Phi-3 Mini 3.8B** ⭐ | 2.2 GB | General use, fast responses | 4 GB |
| **Phi-2 2.7B** | 1.6 GB | Coding, technical tasks | 4 GB |
| **LLaMA 3.2 3B** | 1.9 GB | Multilingual conversations | 4 GB |
| **Mistral 7B** ⭐ | 4.4 GB | Advanced reasoning, coding | 8 GB |
| **LLaMA 3.1 8B** | 4.9 GB | Complex tasks, long context | 12 GB |
| **Gemma 2 9B** | 5.4 GB | Premium quality responses | 12 GB |

### 💾 **Conversation History**
- Automatic conversation saving with SQLite
- Resume conversations anytime
- Search through chat history
- Export conversations

### ⚡ **User Experience**
- Beautiful model selection dialog
- Download progress with speed indicator
- Real-time response streaming
- Keyboard shortcuts (Enter to send)
- Cancel generation anytime
- Change models on the fly

---

## 📋 Requirements

- **OS**: Windows 10 version 1809+ or Windows 11
- **RAM**: 4-16 GB (depends on model choice)
- **Storage**: 2-6 GB free space (for AI model)
- **Internet**: Required for initial model download only

---

## 📦 Installation

### Option 1: Microsoft Store (Recommended)
Coming soon! The MSI installer is ready for Microsoft Store submission.

### Option 2: Download MSI Installer
1. Download the latest `KaiROS-AI-Setup.msi` from [Releases](https://github.com/avikeid2007/KaiROS/releases)
2. Run the installer (supports silent install: `msiexec /i KaiROS-AI-Setup.msi /quiet /qn`)
3. Launch KAIROS from Start Menu or Desktop

### Option 3: Download MSIX Package
1. Download the latest `.msix` from [Releases](https://github.com/avikeid2007/KaiROS/releases)
2. Double-click to install
3. Launch KAIROS from Start Menu

### Option 4: Build from Source
```powershell
# Clone the repository
git clone https://github.com/avikeid2007/KaiROS.git
cd KaiROS

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

> **For Developers**: See [MSI-SETUP-SUMMARY.md](MSI-SETUP-SUMMARY.md) for MSI packaging instructions.

---

## 📸 Screenshots

### Model Selection
Beautiful dialog to choose your AI model

> **Note:** Screenshot coming soon

<!-- ![Model Selection](docs/screenshots/model-selection.png) -->

### Chat Interface
Modern, clean chat experience

> **Note:** Screenshot coming soon

<!-- ![Chat Interface](docs/screenshots/chat-interface.png) -->

### Download Progress
Smooth download experience with progress tracking

> **Note:** Screenshot coming soon

<!-- ![Download Progress](docs/screenshots/download-progress.png) -->

---

## 🔨 Building

### Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (17.8+) with:
  - .NET 8.0 SDK
  - Windows App SDK
  - Universal Windows Platform development workload
- [WiX Toolset v3.11+](https://wixtoolset.org/releases/) (for MSI packaging)

### Build Steps

#### Build Application
1. **Clone the repository**
   ```powershell
   git clone https://github.com/avikeid2007/KaiROS.git
   cd KaiROS
   ```

2. **Open in Visual Studio**
   ```powershell
   start KAIROS.slnx
   ```

3. **Restore NuGet packages**
   - Visual Studio will automatically restore packages
   - Or manually: `dotnet restore`

4. **Build the project**
   - Press `Ctrl+Shift+B` or
   - Build → Build Solution

5. **Run**
   - Press `F5` for debugging
   - Or `Ctrl+F5` for release mode

#### Build MSI Installer

For Microsoft Store submission or distribution:

```powershell
# Navigate to installer directory
cd KAIROS.Installer

# Build MSI for x64
.\Build-MSI.ps1 -Platform x64 -Configuration Release

# Validate MSI
.\Validate-MSI.ps1 -MSIPath "bin\x64\Release\KaiROS-AI-Setup.msi"
```

📖 **Complete MSI Packaging Guide**: [MSI-SETUP-SUMMARY.md](MSI-SETUP-SUMMARY.md)

📋 **Store Submission**: [KAIROS.Installer/SUBMISSION-GUIDE.md](KAIROS.Installer/SUBMISSION-GUIDE.md)

---

## 📁 Project Structure

```
KAIROS/
├── Models/              # Data models (ChatMessage, Conversation, LLMModel)
├── Services/            # Business logic
│   ├── LLMService.cs           # LLamaSharp integration
│   ├── ChatDatabaseService.cs  # SQLite operations
│   ├── ModelDownloaderService.cs # GGUF model downloads
│   └── LLMModelCatalog.cs      # Available models
├── ViewModels/          # MVVM view models
├── Dialogs/             # UI dialogs
├── Data/                # Database context
├── Assets/              # App icons and images
└── Properties/          # App settings
```

---

## ⚙️ Configuration

### Change AI Model
Click the **"Model"** button in the header to switch between models anytime.

### Database Location
Conversations are stored at:
```
%LocalAppData%\KAIROS\chat.db
```

### Model Storage
Downloaded models are cached at:
```
%LocalAppData%\KAIROS\Models\
```

### GPU Acceleration (Optional)
To enable GPU acceleration, modify `LLMService.cs`:
```csharp
var parameters = new ModelParams(modelPath)
{
    ContextSize = 4096,
    GpuLayerCount = 32  // Change from 0 to 32 (or higher)
};
```

---

## 🛠️ Technologies Used

- **[.NET 8](https://dotnet.microsoft.com/)** - Modern .NET framework
- **[WinUI 3](https://microsoft.github.io/microsoft-ui-xaml/)** - Windows UI framework
- **[Windows App SDK](https://learn.microsoft.com/windows/apps/windows-app-sdk/)** - Native Windows features
- **[LLamaSharp](https://github.com/SciSharp/LLamaSharp)** - .NET bindings for llama.cpp
- **[Entity Framework Core](https://docs.microsoft.com/ef/core/)** - Database ORM
- **[SQLite](https://www.sqlite.org/)** - Local database
- **[CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)** - MVVM helpers

---

## 🔄 How It Works

1. **Model Selection**: Choose from curated AI models on first launch
2. **Download**: Model is downloaded once and cached locally
3. **Load**: Model is loaded into memory (RAM)
4. **Chat**: Send messages and receive AI-generated responses
5. **Stream**: Responses stream in real-time, word-by-word
6. **Save**: Conversations automatically saved to local database

### Architecture
- **MVVM Pattern**: Clean separation of concerns
- **Dependency Injection**: Services registered in `App.xaml.cs`
- **Async/Await**: Non-blocking UI with async operations
- **Streaming**: IAsyncEnumerable for real-time text generation

---

## 🔐 Privacy & Security

✅ **100% Local Processing** - All AI computation happens on your device  
✅ **No Cloud Servers** - No data sent to external servers  
✅ **Offline Capable** - Works without internet after model download  
✅ **Local Storage** - Conversations stored only on your PC  
✅ **Open Source** - Audit the code yourself  

---

## ⚠️ Known Issues

- **First load slow**: Large models (7B+) take time to load into RAM
- **High RAM usage**: 7B+ models require 8-16 GB RAM
- **CPU only by default**: GPU acceleration requires configuration

---

## 🗺️ Roadmap

- [ ] GPU acceleration auto-detection
- [x] Export conversations (Markdown) ✅
- [ ] Export conversations to PDF
- [ ] Custom system prompts
- [ ] Temperature/Top-P controls
- [x] Dark/Light theme toggle ✅
- [ ] Multi-conversation tabs
- [x] Search conversations ✅
- [x] Model management (delete cached models) ✅
- [ ] Code syntax highlighting
- [ ] Markdown rendering
- [ ] Voice input (speech-to-text)
- [ ] Conversation statistics dashboard

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### How to Contribute
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines
- Follow C# coding conventions
- Use meaningful commit messages
- Update documentation for new features
- Test on Windows 10 and 11

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- [LLamaSharp](https://github.com/SciSharp/LLamaSharp) - Excellent .NET bindings for llama.cpp
- [llama.cpp](https://github.com/ggerganov/llama.cpp) - Fast LLM inference in C++
- [Microsoft](https://microsoft.com) - WinUI 3 and Windows App SDK
- [Hugging Face](https://huggingface.co/) - Model hosting and distribution
- Model creators: Microsoft (Phi), Meta (LLaMA), Mistral AI, Google (Gemma)

---

## 📧 Contact & Support

- **Issues**: [GitHub Issues](https://github.com/avikeid2007/KaiROS/issues)
- **Discussions**: [GitHub Discussions](https://github.com/avikeid2007/KaiROS/discussions)

---

<div align="center">

Made with ❤️ for the Windows community

**[⬆️ Back to Top](#kairos---ai-chat-assistant)**

</div>
