using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VsLocalizedIntellisense.Models;
using VsLocalizedIntellisense.Models.Configuration;
using VsLocalizedIntellisense.Models.Data;
using VsLocalizedIntellisense.Models.Element;
using VsLocalizedIntellisense.Models.Logger;
using VsLocalizedIntellisense.Models.Service.Application;
using VsLocalizedIntellisense.Models.Service.GitHub;
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

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var appConfiguration = new AppConfiguration(new AppConfigurationInitializeParameters(DateTime.UtcNow));

            var loggerFactory = Logging.Initialize(appConfiguration);
            Logger = loggerFactory.CreateLogger(GetType());
            Logger.LogInformation("START");

            var appFileService = new AppFileService(appConfiguration, loggerFactory);
            var intellisenseVersionData = appFileService.GetIntellisenseVersionData();
            if (intellisenseVersionData == null)
            {
                var appGitHubService = new AppGitHubService(appConfiguration, loggerFactory);
                var intellisenseVersionItems = await appGitHubService.GetVersionItems();
                intellisenseVersionData = new IntellisenseVersionData();
                intellisenseVersionData.VersionItems = intellisenseVersionItems.ToArray();
                appFileService.SaveIntellisenseVersionData(intellisenseVersionData);
            }
            if (intellisenseVersionData == null)
            {
                throw new Exception(nameof(intellisenseVersionData));
            }


            var mainElement = new MainElement(appConfiguration, intellisenseVersionData.VersionItems, loggerFactory);

            var mainViewModel = new MainViewModel(mainElement, appConfiguration, loggerFactory);
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
