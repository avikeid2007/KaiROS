using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Text.Json;

namespace KAIROS.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsFileName = "settings.json";
        private readonly string _settingsPath;

        private AppSettings _settings;

        public ElementTheme Theme
        {
            get => _settings.Theme;
            set
            {
                _settings.Theme = value;
                SaveSettings();
            }
        }

        public SettingsService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KAIROS");
            
            Directory.CreateDirectory(appDataPath);
            _settingsPath = Path.Combine(appDataPath, SettingsFileName);
            
            _settings = new AppSettings();
            LoadSettings();
        }

        public void SaveSettings()
        {
            try
            {
                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception)
            {
                // Silently fail - settings are not critical
            }
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    _settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception)
            {
                // If loading fails, use default settings
                _settings = new AppSettings();
            }
        }

        private class AppSettings
        {
            public ElementTheme Theme { get; set; } = ElementTheme.Default;
        }
    }
}
