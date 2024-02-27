using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VsLocalizedIntellisense.Models;
using VsLocalizedIntellisense.Models.Configuration;
using VsLocalizedIntellisense.Models.Element;
using VsLocalizedIntellisense.Models.Logger;
using VsLocalizedIntellisense.ViewModels;
using VsLocalizedIntellisense.Views;

namespace VsLocalizedIntellisense
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        #region property

        private ILogger Logger { get; set; }

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var appConfiguration = new AppConfiguration(new AppConfigurationInitializeParameters(DateTime.UtcNow));

            var loggerFactory = Logging.Initialize(appConfiguration);
            Logger = loggerFactory.CreateLogger(GetType());
            Logger.LogInformation("START");

            var mainElement = new MainElement(appConfiguration, Logging.Instance);
            mainElement.LoadAsync();

            var mainViewModel = new MainViewModel(mainElement, Logging.Instance);
            var mainView = new MainWindow();

            MainWindow = mainView;
            MainWindow.DataContext = mainViewModel;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Logger?.LogInformation("EXIT");
            Logging.Shutdown();
        }

        #endregion
    }
}
