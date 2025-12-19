# Contributing to KaiROS

Thank you for your interest in contributing to KaiROS! This document provides guidelines for contributing to the project.

## 🤝 How to Contribute

### Reporting Bugs

If you find a bug, please create an issue on GitHub with:
- A clear, descriptive title
- Steps to reproduce the issue
- Expected behavior
- Actual behavior
- Your environment (Windows version, RAM, model being used)
- Screenshots if applicable

### Suggesting Enhancements

Enhancement suggestions are welcome! Please create an issue with:
- A clear, descriptive title
- Detailed description of the proposed feature
- Why this enhancement would be useful
- Any potential implementation ideas

### Pull Requests

1. **Fork the repository**
   ```bash
   # Fork via GitHub UI, then clone your fork
   git clone https://github.com/<your-username>/KaiROS.git
   cd KaiROS
   ```

2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make your changes**
   - Follow the existing code style
   - Write meaningful commit messages
   - Test your changes thoroughly

4. **Commit your changes**
   ```bash
   git add .
   git commit -m "feat: Add your feature description"
   ```

5. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Open a Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Select your feature branch
   - Describe your changes

## 📋 Development Guidelines

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add comments for complex logic
- Keep methods focused and concise
- Use async/await for I/O operations

### Commit Messages

Use conventional commit format:
```
type: brief description

Optional detailed description
```

Types:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

### Testing

- Test your changes on Windows 10 and Windows 11 if possible
- Test with different AI models
- Verify UI responsiveness
- Check for memory leaks with large models
- Test edge cases

### Building the Project

1. **Prerequisites**
   - Visual Studio 2022 (17.8+)
   - .NET 8.0 SDK
   - Windows App SDK

2. **Build**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run**
   ```bash
   dotnet run
   ```

## 🎯 Areas for Contribution

We especially welcome contributions in these areas:

- **GPU Acceleration**: Implement auto-detection and configuration
- **UI Improvements**: Enhance the user interface and experience
- **Model Management**: Add ability to delete/manage cached models
- **Export Features**: Add conversation export (PDF, Markdown)
- **Markdown Rendering**: Render markdown in chat responses
- **Code Highlighting**: Syntax highlighting for code blocks
- **Performance**: Optimize loading times and memory usage
- **Testing**: Add unit and integration tests
- **Documentation**: Improve guides and documentation

## 📝 Documentation

When adding features:
- Update README.md if needed
- Add inline code comments
- Update relevant .md guides
- Add screenshots for UI changes

## 🔒 Security

If you discover a security vulnerability:
- **DO NOT** open a public issue
- Use GitHub's security advisory feature
- Or contact the maintainers privately

## ❓ Questions?

- Check existing issues and discussions
- Create a new discussion for questions
- Be respectful and constructive

## 📜 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to KaiROS! 🙏
