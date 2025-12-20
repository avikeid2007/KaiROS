using KAIROS.Models;
using KAIROS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAIROS.Dialogs
{
    public sealed partial class ConversationHistoryDialog : ContentDialog
    {
        private readonly IChatDatabaseService _databaseService;
        private List<Conversation> _allConversations = new();
        private List<Conversation> _filteredConversations = new();

        public Conversation? SelectedConversation { get; private set; }

        public ConversationHistoryDialog(IChatDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            
            Loaded += ConversationHistoryDialog_Loaded;
        }

        private async void ConversationHistoryDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadConversationsAsync();
        }

        private async Task LoadConversationsAsync()
        {
            LoadingRing.Visibility = Visibility.Visible;
            ConversationsPanel.Visibility = Visibility.Collapsed;
            NoResultsTextBlock.Visibility = Visibility.Collapsed;

            try
            {
                // Get all conversations from database
                _allConversations = await _databaseService.GetAllConversationsAsync();
                _filteredConversations = _allConversations.OrderByDescending(c => c.LastUpdated).ToList();
                
                DisplayConversations();
            }
            catch (Exception)
            {
                NoResultsTextBlock.Text = "Error loading conversations.";
                NoResultsTextBlock.Visibility = Visibility.Visible;
            }
            finally
            {
                LoadingRing.Visibility = Visibility.Collapsed;
            }
        }

        private void DisplayConversations()
        {
            ConversationsPanel.Children.Clear();

            if (_filteredConversations.Count == 0)
            {
                ConversationsPanel.Visibility = Visibility.Collapsed;
                NoResultsTextBlock.Visibility = Visibility.Visible;
                return;
            }

            ConversationsPanel.Visibility = Visibility.Visible;
            NoResultsTextBlock.Visibility = Visibility.Collapsed;

            foreach (var conversation in _filteredConversations)
            {
                var card = CreateConversationCard(conversation);
                ConversationsPanel.Children.Add(card);
            }
        }

        private Border CreateConversationCard(Conversation conversation)
        {
            var messageCount = conversation.Messages?.Count ?? 0;
            var lastMessage = conversation.Messages?.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Content ?? "";
            var preview = lastMessage.Length > 100 ? lastMessage.Substring(0, 100) + "..." : lastMessage;

            var radioButton = new RadioButton
            {
                GroupName = "ConversationSelection",
                Tag = conversation,
                Content = new StackPanel
                {
                    Spacing = 4,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = conversation.Title,
                            Style = Application.Current.Resources["BodyStrongTextBlockStyle"] as Style,
                            TextWrapping = TextWrapping.NoWrap,
                            TextTrimming = TextTrimming.CharacterEllipsis
                        },
                        new TextBlock
                        {
                            Text = preview,
                            Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style,
                            Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as Microsoft.UI.Xaml.Media.Brush,
                            TextWrapping = TextWrapping.NoWrap,
                            TextTrimming = TextTrimming.CharacterEllipsis,
                            Margin = new Thickness(0, 4, 0, 0)
                        },
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 12,
                            Margin = new Thickness(0, 4, 0, 0),
                            Children =
                            {
                                new TextBlock
                                {
                                    Text = $"📅 {conversation.LastUpdated:MMM dd, yyyy}",
                                    Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style,
                                    Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as Microsoft.UI.Xaml.Media.Brush
                                },
                                new TextBlock
                                {
                                    Text = $"💬 {messageCount} messages",
                                    Style = Application.Current.Resources["CaptionTextBlockStyle"] as Style,
                                    Foreground = Application.Current.Resources["TextFillColorSecondaryBrush"] as Microsoft.UI.Xaml.Media.Brush
                                }
                            }
                        }
                    }
                }
            };

            radioButton.Checked += (s, e) =>
            {
                if (radioButton.Tag is Conversation conv)
                {
                    SelectedConversation = conv;
                }
            };

            var card = new Border
            {
                Background = Application.Current.Resources["CardBackgroundFillColorDefaultBrush"] as Microsoft.UI.Xaml.Media.Brush,
                BorderBrush = Application.Current.Resources["CardStrokeColorDefaultBrush"] as Microsoft.UI.Xaml.Media.Brush,
                BorderThickness = new Thickness(1),
                CornerRadius = new Microsoft.UI.Xaml.CornerRadius(8),
                Padding = new Thickness(12),
                Child = radioButton
            };

            return card;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text?.Trim().ToLowerInvariant() ?? "";

            if (string.IsNullOrEmpty(searchText))
            {
                _filteredConversations = _allConversations.OrderByDescending(c => c.LastUpdated).ToList();
            }
            else
            {
                _filteredConversations = _allConversations
                    .Where(c => 
                        c.Title.ToLowerInvariant().Contains(searchText) ||
                        (c.Messages?.Any(m => m.Content.ToLowerInvariant().Contains(searchText)) ?? false))
                    .OrderByDescending(c => c.LastUpdated)
                    .ToList();
            }

            DisplayConversations();
        }
    }
}
