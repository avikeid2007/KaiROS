using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace KAIROS.ViewModels
{
    public partial class ChatMessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string content = string.Empty;

        [ObservableProperty]
        private string role = string.Empty;

        [ObservableProperty]
        private DateTime timestamp;

        [ObservableProperty]
        private bool isUser;

        public ChatMessageViewModel(string content, string role, DateTime timestamp)
        {
            this.content = content;
            this.role = role;
            this.timestamp = timestamp;
            this.isUser = role.Equals("user", StringComparison.OrdinalIgnoreCase);
        }
    }
}
