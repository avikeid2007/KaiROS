using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KAIROS.Models;
using KAIROS.Services;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAIROS.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IChatDatabaseService _databaseService;
        private readonly ILLMService _llmService;
        private readonly IModelDownloaderService _modelDownloaderService;
        private readonly DispatcherQueue _dispatcherQueue;
        private CancellationTokenSource? _cancellationTokenSource;

        [ObservableProperty]
        private ObservableCollection<ChatMessageViewModel> messages = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
        private string userInput = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isModelReady;

        [ObservableProperty]
        private string statusMessage = "Initializing...";

        [ObservableProperty]
        private double downloadProgress;

        [ObservableProperty]
        private bool isDownloading;

        [ObservableProperty]
        private string currentConversationTitle = "New Conversation";

        private Conversation? _currentConversation;

        public MainViewModel(
            IChatDatabaseService databaseService,
            ILLMService llmService,
            IModelDownloaderService modelDownloaderService,
            DispatcherQueue dispatcherQueue)
        {
            _databaseService = databaseService;
            _llmService = llmService;
            _modelDownloaderService = modelDownloaderService;
            _dispatcherQueue = dispatcherQueue;
        }

        public async Task InitializeAsync(Models.LLMModel? selectedModel = null)
        {
            try
            {
                StatusMessage = "Initializing database...";
                await _databaseService.InitializeDatabaseAsync();

                // Create a new conversation
                _currentConversation = await _databaseService.CreateConversationAsync("New Conversation");
                CurrentConversationTitle = _currentConversation.Title;

                // If no model selected, the caller should show model selection dialog
                if (selectedModel == null)
                {
                    StatusMessage = "Please select a model to continue...";
                    return;
                }

                StatusMessage = "Checking for model...";

                if (!_modelDownloaderService.IsModelDownloaded(selectedModel.Name))
                {
                    StatusMessage = $"Downloading {selectedModel.DisplayName}...";
                    IsDownloading = true;

                    var progress = new Progress<double>(p =>
                    {
                        _dispatcherQueue.TryEnqueue(() =>
                        {
                            DownloadProgress = p;
                            StatusMessage = $"Downloading {selectedModel.DisplayName}: {p:F1}%";
                        });
                    });

                    await _modelDownloaderService.DownloadModelAsync(
                        selectedModel.DownloadUrl, 
                        selectedModel.Name, 
                        progress);
                    
                    IsDownloading = false;
                }

                StatusMessage = $"Loading {selectedModel.DisplayName}...";
                var modelPath = _modelDownloaderService.GetModelPath(selectedModel.Name);
                await _llmService.InitializeAsync(modelPath);

                IsModelReady = true;
                StatusMessage = "Ready! Start chatting...";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                IsModelReady = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanSendMessage))]
        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(UserInput) || _currentConversation == null)
                return;

            var userMessage = UserInput.Trim();
            UserInput = string.Empty;

            // Add user message to UI
            var userMessageViewModel = new ChatMessageViewModel(userMessage, "user", DateTime.Now);
            Messages.Add(userMessageViewModel);

            // Save to database
            await _databaseService.AddMessageAsync(_currentConversation.Id, userMessage, "user");

            // Update conversation title if it's the first message
            if (Messages.Count == 1)
            {
                var title = userMessage.Length > 30 ? userMessage.Substring(0, 30) + "..." : userMessage;
                await _databaseService.UpdateConversationTitleAsync(_currentConversation.Id, title);
                CurrentConversationTitle = title;
            }

            IsLoading = true;
            StatusMessage = "Thinking...";

            // Prepare assistant message
            var assistantMessage = new ChatMessageViewModel(string.Empty, "assistant", DateTime.Now);
            Messages.Add(assistantMessage);

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                
                // Get all messages for context
                var chatHistory = (await _databaseService.GetConversationAsync(_currentConversation.Id))?.Messages.ToList()
                    ?? new System.Collections.Generic.List<ChatMessage>();

                // Stream the response
                await foreach (var token in _llmService.GenerateResponseAsync(chatHistory, _cancellationTokenSource.Token))
                {
                    _dispatcherQueue.TryEnqueue(() =>
                    {
                        assistantMessage.Content += token;
                    });
                }

                // Save assistant message to database
                if (!string.IsNullOrWhiteSpace(assistantMessage.Content))
                {
                    await _databaseService.AddMessageAsync(_currentConversation.Id, assistantMessage.Content, "assistant");
                }

                StatusMessage = "Ready! Start chatting...";
            }
            catch (OperationCanceledException)
            {
                assistantMessage.Content += " [Cancelled]";
                StatusMessage = "Response cancelled.";
            }
            catch (Exception ex)
            {
                assistantMessage.Content = $"Error: {ex.Message}";
                StatusMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrWhiteSpace(UserInput) && IsModelReady && !IsLoading;
        }

        [RelayCommand]
        private void StopGeneration()
        {
            _cancellationTokenSource?.Cancel();
        }

        [RelayCommand]
        private async Task NewConversationAsync()
        {
            Messages.Clear();
            _currentConversation = await _databaseService.CreateConversationAsync("New Conversation");
            CurrentConversationTitle = "New Conversation";
            StatusMessage = "New conversation started.";
        }

    }
}
