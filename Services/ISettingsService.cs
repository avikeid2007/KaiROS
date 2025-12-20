using Microsoft.UI.Xaml;

namespace KAIROS.Services
{
    public interface ISettingsService
    {
        ElementTheme Theme { get; set; }
        void SaveSettings();
        void LoadSettings();
    }
}
