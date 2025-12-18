using KAIROS.Models;
using KAIROS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace KAIROS.Dialogs
{
    public sealed partial class ModelSelectionDialog : ContentDialog
    {
        public LLMModel? SelectedModel { get; private set; }

        public ModelSelectionDialog()
        {
            InitializeComponent();
            LoadModels();
        }

        private void LoadModels()
        {
            var allModels = LLMModelCatalog.GetAvailableModels();

            // Populate small models
            var smallModels = allModels.Where(m => m.Category == "small").ToList();
            SmallModelsItems.ItemsSource = smallModels;

            // Populate medium models
            var mediumModels = allModels.Where(m => m.Category == "medium").ToList();
            MediumModelsItems.ItemsSource = mediumModels;

            // Populate large models
            var largeModels = allModels.Where(m => m.Category == "large").ToList();
            LargeModelsItems.ItemsSource = largeModels;

            // Pre-select the first recommended model
            var recommendedModel = allModels.FirstOrDefault(m => m.IsRecommended);
            if (recommendedModel != null)
            {
                SelectedModel = recommendedModel;
            }
        }

        private void ModelRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Tag is LLMModel model)
            {
                SelectedModel = model;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Validate selection
            if (SelectedModel == null)
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            SelectedModel = null;
        }
    }
}
