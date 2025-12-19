using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.IO;

namespace KAIROS.Dialogs
{
    public sealed partial class SettingsDialog : ContentDialog
    {
        private readonly Services.ISettingsService _settingsService;
        private readonly Services.IModelDownloaderService _modelDownloaderService;
        private readonly Window _window;

        public SettingsDialog(Services.ISettingsService settingsService, Services.IModelDownloaderService modelDownloaderService, Window window)
        {
            InitializeComponent();
            _settingsService = settingsService;
            _modelDownloaderService = modelDownloaderService;
            _window = window;
            
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Set theme
            switch (_settingsService.Theme)
            {
                case ElementTheme.Default:
                    ThemeComboBox.SelectedIndex = 0;
                    break;
                case ElementTheme.Light:
                    ThemeComboBox.SelectedIndex = 1;
                    break;
                case ElementTheme.Dark:
                    ThemeComboBox.SelectedIndex = 2;
                    break;
            }

            // Set data locations
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KAIROS");
            
            DataLocationTextBlock.Text = appDataPath;
            ModelsLocationTextBlock.Text = Path.Combine(appDataPath, "Models");
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                ElementTheme theme = tag switch
                {
                    "Light" => ElementTheme.Light,
                    "Dark" => ElementTheme.Dark,
                    _ => ElementTheme.Default
                };

                _settingsService.Theme = theme;
                
                // Apply theme to current window
                if (_window.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = theme;
                }
            }
        }

        private void OpenDataFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "KAIROS");
                
                Directory.CreateDirectory(appDataPath);
                Process.Start("explorer.exe", appDataPath);
            }
            catch
            {
                // Silently fail if we can't open the folder
            }
        }

        private async void ManageModels_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var managementDialog = new ModelManagementDialog(_modelDownloaderService)
                {
                    XamlRoot = this.XamlRoot
                };

                await managementDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to open model management: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }
    }
}
