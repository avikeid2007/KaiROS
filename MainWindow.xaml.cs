using KAIROS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using Microsoft.UI.Xaml.Data;

namespace KAIROS
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        private bool _initialized = false;

        public MainWindow(MainViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            
            Title = "KAIROS - AI Chat Assistant";
            
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
