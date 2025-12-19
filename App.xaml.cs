using KAIROS.Services;
using KAIROS.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;

namespace KAIROS
{
    public partial class App : Application
    {
        private Window? _window;
        private IServiceProvider? _serviceProvider;

        public App()
        {
            InitializeComponent();
            ConfigureServices();
        }

<<<<<<< HEAD
=======
        public T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider not initialized");
            }
            
            return _serviceProvider.GetRequiredService<T>();
        }

>>>>>>> origin/main
        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<IChatDatabaseService, ChatDatabaseService>();
            services.AddSingleton<ILLMService, LLMService>();
            services.AddSingleton<IModelDownloaderService, ModelDownloaderService>();
<<<<<<< HEAD
=======
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IConversationExportService, ConversationExportService>();
>>>>>>> origin/main
            
            // Register DispatcherQueue
            services.AddSingleton(DispatcherQueue.GetForCurrentThread());

            // Register ViewModels
            services.AddTransient<MainViewModel>();

            // Register MainWindow
            services.AddTransient<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider not initialized");
            }

            _window = _serviceProvider.GetRequiredService<MainWindow>();
            _window.Activate();
        }
    }
}
