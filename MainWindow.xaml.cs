using KAIROS.ViewModels;
<<<<<<< HEAD
=======
using KAIROS.Services;
>>>>>>> origin/main
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using Microsoft.UI.Xaml.Data;
<<<<<<< HEAD
=======
using Microsoft.Extensions.DependencyInjection;
>>>>>>> origin/main

namespace KAIROS
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }
<<<<<<< HEAD

        private bool _initialized = false;

        public MainWindow(MainViewModel viewModel)
        {
            ViewModel = viewModel;
=======
        private readonly ISettingsService _settingsService;

        private bool _initialized = false;

        public MainWindow(MainViewModel viewModel, ISettingsService settingsService)
        {
            ViewModel = viewModel;
            _settingsService = settingsService;
>>>>>>> origin/main
            InitializeComponent();
            
            Title = "KAIROS - AI Chat Assistant";
            
<<<<<<< HEAD
=======
            // Apply saved theme
            if (this.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = _settingsService.Theme;
            }

            // Subscribe to message added event for auto-scroll
            ViewModel.MessageAdded += (s, e) =>
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    // Scroll to bottom when new message is added
                    ChatScrollViewer.ChangeView(null, ChatScrollViewer.ScrollableHeight, null);
                });
            };
            
>>>>>>> origin/main
            // Use DispatcherQueue to defer initialization until after window is fully loaded
            this.DispatcherQueue.TryEnqueue(async () =>
            {
                if (!_initialized)
                {
                    _initialized = true;
                    
                    // Small delay to ensure UI is fully loaded
                    await System.Threading.Tasks.Task.Delay(100);
                    
                    await ShowModelSelectionAndInitializeAsync();
                }
            });
        }

        private async System.Threading.Tasks.Task ShowModelSelectionAndInitializeAsync()
        {
            try
            {
                // Ensure we have a valid XamlRoot
                if (this.Content == null || this.Content.XamlRoot == null)
                {
                    ViewModel.StatusMessage = "Window not ready. Please try again.";
                    return;
                }

                // Show model selection dialog
                var dialog = new Dialogs.ModelSelectionDialog
                {
                    XamlRoot = this.Content.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary && dialog.SelectedModel != null)
                {
                    // User selected a model, initialize with it
                    await ViewModel.InitializeAsync(dialog.SelectedModel);
                }
                else
                {
                    // User cancelled, show message
                    ViewModel.StatusMessage = "No model selected. Click 'Model' button to select one.";
                }
            }
            catch (Exception ex)
            {
                ViewModel.StatusMessage = $"Error showing model selection: {ex.Message}";
            }
        }

        private async void SelectModel_Click(object sender, RoutedEventArgs e)
        {
            await ShowModelSelectionAndInitializeAsync();
        }

        private void SendMessage_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.UserInput) && ViewModel.IsModelReady && !ViewModel.IsLoading)
            {
                args.Handled = true;
                _ = ViewModel.SendMessageCommand.ExecuteAsync(null);
            }
        }
<<<<<<< HEAD
=======

        private async void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Content?.XamlRoot == null)
                    return;

                var modelDownloaderService = ((App)Application.Current).GetService<IModelDownloaderService>();
                
                var dialog = new Dialogs.SettingsDialog(_settingsService, modelDownloaderService, this)
                {
                    XamlRoot = this.Content.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ViewModel.StatusMessage = $"Error opening settings: {ex.Message}";
            }
        }

        private async void ExportConversation_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ExportConversationCommand.ExecuteAsync(null);
        }

        private async void OpenHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Content?.XamlRoot == null)
                    return;

                var databaseService = ((App)Application.Current).GetService<IChatDatabaseService>();
                
                var dialog = new Dialogs.ConversationHistoryDialog(databaseService)
                {
                    XamlRoot = this.Content.XamlRoot
                };

                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary && dialog.SelectedConversation != null)
                {
                    await ViewModel.LoadConversationAsync(dialog.SelectedConversation);
                }
            }
            catch (Exception ex)
            {
                ViewModel.StatusMessage = $"Error opening history: {ex.Message}";
            }
        }

        // Keyboard accelerator handlers
        private void NewConversation_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (ViewModel.IsModelReady)
            {
                args.Handled = true;
                _ = ViewModel.NewConversationCommand.ExecuteAsync(null);
            }
        }

        private void History_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (ViewModel.IsModelReady)
            {
                args.Handled = true;
                OpenHistory_Click(sender, null!);
            }
        }

        private void Export_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (ViewModel.IsModelReady)
            {
                args.Handled = true;
                ExportConversation_Click(sender, null!);
            }
        }

        private void Settings_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            OpenSettings_Click(sender, null!);
        }

        private async void OpenHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Content?.XamlRoot == null)
                    return;

                var dialog = new Dialogs.HelpDialog
                {
                    XamlRoot = this.Content.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ViewModel.StatusMessage = $"Error opening help: {ex.Message}";
            }
        }

        private void Help_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
            OpenHelp_Click(sender, null!);
        }

        private void CopyMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is string content)
                {
                    var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
                    dataPackage.SetText(content);
                    Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                    
                    // Show a brief feedback
                    ViewModel.StatusMessage = "Message copied to clipboard";
                    
                    // Reset status message after 2 seconds
                    var timer = new System.Threading.Timer(_ =>
                    {
                        DispatcherQueue.TryEnqueue(() =>
                        {
                            if (ViewModel.StatusMessage == "Message copied to clipboard")
                            {
                                ViewModel.StatusMessage = ViewModel.IsModelReady ? "Ready! Start chatting..." : "Please select a model to continue...";
                            }
                        });
                    }, null, 2000, System.Threading.Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                ViewModel.StatusMessage = $"Copy failed: {ex.Message}";
            }
        }
>>>>>>> origin/main
    }

    // Converter for inverting boolean to visibility
    public class InverseBoolToVisibilityConverter : Microsoft.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
