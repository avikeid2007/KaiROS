# New Features Guide

This document describes the new features added to KAIROS AI Chat Assistant.

## 📋 Table of Contents
- [Theme Toggle](#theme-toggle)
- [Export Conversations](#export-conversations)
- [Model Management](#model-management)
- [Conversation History & Search](#conversation-history--search)
- [Keyboard Shortcuts](#keyboard-shortcuts)
- [Settings Dialog](#settings-dialog)

---

## 🎨 Theme Toggle

### Description
Switch between Light, Dark, and System Default themes to match your preference and environment.

### How to Use
1. Click the **Settings** button in the header (or press `Ctrl+,`)
2. In the **Appearance** section, select your preferred theme:
   - **Use System Setting** - Follows Windows theme
   - **Light** - Light theme
   - **Dark** - Dark theme
3. Theme changes are applied immediately
4. Your preference is saved and persists across app restarts

### Technical Details
- Theme preference stored in: `%LocalAppData%\KAIROS\settings.json`
- Uses WinUI 3's `ElementTheme` system
- Supports Mica backdrop in all themes

---

## 📤 Export Conversations

### Description
Export your conversations to Markdown format for archival, sharing, or documentation purposes.

### How to Use
1. Click the **Export** button in the header (or press `Ctrl+E`)
2. The current conversation is automatically exported
3. File is saved to your **Documents** folder with format:
   ```
   ConversationTitle_YYYYMMDD_HHMMSS.md
   ```

### Export Format
The exported Markdown file includes:
- **Title**: Conversation title as heading
- **Metadata**: Created date, last updated, message count
- **Messages**: All messages with timestamps and role indicators
  - 👤 **You** - Your messages
  - 🤖 **AI Assistant** - AI responses
- **Footer**: Export timestamp

### Example Export
```markdown
# My Conversation

**Created:** 2024-01-15 10:30:00
**Last Updated:** 2024-01-15 10:45:00
**Messages:** 8

---

### 👤 **You**
*2024-01-15 10:30:15*

What is quantum computing?

---

### 🤖 **AI Assistant**
*2024-01-15 10:30:25*

Quantum computing is a revolutionary computing paradigm...

---
```

---

## 🗂️ Model Management

### Description
View all downloaded AI models and delete ones you no longer need to free up disk space.

### How to Use
1. Click **Settings** button (or press `Ctrl+,`)
2. Click **Manage Models** button in the Storage section
3. View all downloaded models with:
   - Model name
   - File size
   - Download date
4. Click **Delete** on any model to remove it
5. Confirm deletion in the dialog

### Features
- **Total Storage Display**: See total space used by all models
- **Delete Confirmation**: Prevents accidental deletion
- **Real-time Updates**: List refreshes after each deletion
- **Safe Operation**: Only deletes model files, not conversations

### Storage Locations
- Models: `%LocalAppData%\KAIROS\Models\`
- Database: `%LocalAppData%\KAIROS\chat.db`

---

## 📜 Conversation History & Search

### Description
Browse all your past conversations and search through them by title or content.

### How to Use
1. Click the **History** button in the header (or press `Ctrl+H`)
2. Browse the list of conversations, showing:
   - Conversation title
   - Last message preview
   - Last updated date
   - Message count
3. Use the search box to filter conversations:
   - Searches in conversation titles
   - Searches in message content
4. Select a conversation (click the radio button)
5. Click **Load Selected** to open that conversation

### Features
- **Real-time Search**: Results update as you type
- **Smart Preview**: Shows last message preview (truncated at 100 chars)
- **Sorted by Date**: Most recent conversations first
- **Message Count**: See how many messages in each conversation
- **Fast Loading**: Efficient database queries

### Search Tips
- Search is case-insensitive
- Matches any part of title or message content
- Clear search box to see all conversations
- No results? The "No conversations found" message appears

---

## ⌨️ Keyboard Shortcuts

### Description
Convenient keyboard shortcuts for common actions to improve productivity.

### Available Shortcuts

| Shortcut | Action | Description |
|----------|--------|-------------|
| `Ctrl+N` | New Conversation | Start a new conversation |
| `Ctrl+H` | History | Open conversation history |
| `Ctrl+E` | Export | Export current conversation |
| `Ctrl+,` | Settings | Open settings dialog |
| `Enter` | Send Message | Send your message to AI |
| `Shift+Enter` | New Line | Add a new line in message input |

### Tips
- All shortcuts work when the model is ready
- Shortcuts are context-aware (e.g., can't export if no conversation)
- Message input shortcuts only work when input box is focused

---

## ⚙️ Settings Dialog

### Description
Centralized settings interface for configuring KAIROS.

### How to Access
- Click **Settings** button in header
- Or press `Ctrl+,`

### Sections

#### 1. Appearance
- **Theme Selection**: Choose Light, Dark, or System Default
- **Instant Apply**: Changes take effect immediately

#### 2. About
- **App Information**: Name, version, description
- **App Icon**: Visual branding

#### 3. Storage
- **Manage Models**: Button to open Model Management dialog
- **Data Location**: Path to conversation database
- **Models Location**: Path to downloaded models
- **Open Data Folder**: Quick access to data directory

### Settings Persistence
- Settings are saved to: `%LocalAppData%\KAIROS\settings.json`
- Automatically loaded on app start
- Settings survive app updates

---

## 🎯 Quick Reference

### Common Tasks

**Start a new conversation:**
- Click "New Chat" button or press `Ctrl+N`

**Switch themes:**
- Settings → Appearance → Select theme

**Find old conversation:**
- Click "History" or press `Ctrl+H` → Search → Select → Load

**Export conversation:**
- Click "Export" or press `Ctrl+E` → Check Documents folder

**Free up disk space:**
- Settings → Manage Models → Delete unused models

**Check storage usage:**
- Settings → Manage Models → See total storage

---

## 💡 Tips & Best Practices

1. **Regular Exports**: Export important conversations periodically for backup
2. **Theme Switching**: Use Dark theme for nighttime use to reduce eye strain
3. **Model Management**: Delete old model versions after upgrading
4. **Conversation History**: Use search to quickly find specific topics
5. **Keyboard Shortcuts**: Learn shortcuts for faster workflow

---

## 🐛 Troubleshooting

### Export button is disabled
- Make sure a model is loaded and ready
- Start a new conversation if needed

### Can't find exported file
- Check your Documents folder
- File name includes conversation title and timestamp
- Search for "*.md" files

### Model deletion failed
- Make sure the model is not currently loaded
- Close and restart the app
- Check file permissions

### Settings not saving
- Check write permissions for `%LocalAppData%\KAIROS\`
- Ensure disk has free space
- Try running as administrator

---

## 🔄 Future Enhancements

Planned features for future releases:
- Multi-conversation tabs
- Code syntax highlighting
- Custom system prompts
- Export to PDF
- Conversation statistics
- Voice input (speech-to-text)
- Markdown rendering in chat

---

*Last Updated: 2024-12-19*
