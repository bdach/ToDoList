using System;
using System.IO;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Ticketron.App.Persistence;
using Ticketron.App.ViewModels;
using Ticketron.DB;
using Ticketron.DB.Repositories;

namespace Ticketron.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The current <see cref="App"/> instance in use.
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// The global state of the application instance.
        /// </summary>
        public AppViewModel State { get; private set; } = null!;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services.
        /// </summary>
        public IServiceProvider Services { get; }

        private Window _window = null!;
        private AppStatePersistenceManager _appStatePersistenceManager = null!;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            var storageDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ticketron");
            Directory.CreateDirectory(storageDirectory);

            var services = new ServiceCollection();
            services
                .AddTicketronDatabase(Path.Combine(storageDirectory, "data.db"))
                .AddPersistenceManagers();
            Services = services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var migrationRunner = Services.GetRequiredService<IMigrationRunner>();
            migrationRunner.MigrateUp();

            _appStatePersistenceManager = Services.GetRequiredService<AppStatePersistenceManager>();
            State = _appStatePersistenceManager.LoadAppState().Result;

            _window = new MainWindow();
            _window.Activate();
        }
    }
}
