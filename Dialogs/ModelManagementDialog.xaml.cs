using KAIROS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KAIROS.Dialogs
{
    public sealed partial class ModelManagementDialog : ContentDialog
    {
        private readonly IModelDownloaderService _modelDownloaderService;
        private string _modelsDirectory;

        public ModelManagementDialog(IModelDownloaderService modelDownloaderService)
        {
            InitializeComponent();
            _modelDownloaderService = modelDownloaderService;
            
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KAIROS");
            _modelsDirectory = Path.Combine(appDataPath, "Models");
            
            LoadModels();
        }

        private void LoadModels()
        {
            ModelsStackPanel.Children.Clear();
            
            if (!Directory.Exists(_modelsDirectory))
            {
                NoModelsTextBlock.Visibility = Visibility.Visible;
                TotalStorageTextBlock.Text = "0 MB";
                return;
            }

            var modelFiles = Directory.GetFiles(_modelsDirectory, "*.gguf");
            
            if (modelFiles.Length == 0)
            {
                NoModelsTextBlock.Visibility = Visibility.Visible;
                TotalStorageTextBlock.Text = "0 MB";
                return;
            }

            NoModelsTextBlock.Visibility = Visibility.Collapsed;
            
            long totalSize = 0;
            var modelInfos = new List<(string name, long size, DateTime modified)>();

            foreach (var modelFile in modelFiles)
            {
                var fileInfo = new FileInfo(modelFile);
                totalSize += fileInfo.Length;
                modelInfos.Add((fileInfo.Name, fileInfo.Length, fileInfo.LastWriteTime));
            }

            // Update total storage
            TotalStorageTextBlock.Text = FormatBytes(totalSize);

            // Add model cards
            foreach (var (name, size, modified) in modelInfos.OrderByDescending(m => m.size))
            {
                var card = CreateModelCard(name, size, modified);
                ModelsStackPanel.Children.Add(card);
            }
        }

        private Border CreateModelCard(string modelName, long size, DateTime modified)
        {
            var deleteButton = new Button
            {
                Content = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 8,
                    Children =
                    {
                        new FontIcon { Glyph = "\uE74D", FontSize = 16 },
                        new TextBlock { Text = "Delete" }
                    }
                },
                Tag = modelName,
                Style = Application.Current.Resources["AccentButtonStyle"] as Style
            };
            deleteButton.Click += DeleteModel_Click;

            var card = new Border
            {
                Background = Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] as Microsoft.UI.Xaml.Media.Brush,
                BorderBrush = Application.Current.Resources["CardStrokeColorDefaultBrush"] as Microsoft.UI.Xaml.Media.Brush,
                BorderThickness = new Thickness(1),
                CornerRadius = new Microsoft.UI.Xaml.CornerRadius(8),
                Padding = new Thickness(16),
                Child = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    Children =
                    {
                        new StackPanel
                        {
                            Spacing = 4,
                            Children =
                            {
                                new TextBlock
                                {
                                    Text = modelName.Replace(".gguf", ""),
                                    Style = Application.Current.Resources["BodyStrongTextBlockStyle"] as Style,
                                    TextWrapping = TextWrapping.Wrap
                                },
                                new TextBlock
                                {
                                    Text = $"Size: {FormatBytes(size)}",
                                    Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style,
                                    Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as Microsoft.UI.Xaml.Media.Brush
                                },
                                new TextBlock
                                {
                                    Text = $"Downloaded: {modified:yyyy-MM-dd HH:mm}",
                                    Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style,
                                    Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as Microsoft.UI.Xaml.Media.Brush
                                }
                            }
                        }
                    }
                }
            };

            if (card.Child is Grid grid && grid.Children.Count > 0)
            {
                Grid.SetColumn(deleteButton, 1);
                deleteButton.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(deleteButton);
            }

            return card;
        }

        private async void DeleteModel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string modelName)
            {
                // Confirm deletion
                var confirmDialog = new ContentDialog
                {
                    Title = "Delete Model",
                    Content = $"Are you sure you want to delete '{modelName}'? This action cannot be undone.",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };

                var result = await confirmDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        var modelPath = Path.Combine(_modelsDirectory, modelName);
                        if (File.Exists(modelPath))
                        {
                            File.Delete(modelPath);
                            LoadModels(); // Refresh the list
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = $"Failed to delete model: {ex.Message}",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
