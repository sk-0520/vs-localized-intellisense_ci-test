using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;
using VsLocalizedIntellisense.Models.Logger;
using VsLocalizedIntellisense.Models.Service.GitHub;

namespace VsLocalizedIntellisense.Models.Element
{
    public class MainElement : ElementBase
    {
        #region variable

        private string _installRootDirectoryPath;

        #endregion

        public MainElement(AppConfiguration configuration, IReadOnlyList<GitHubContentResponseItem> intellisenseItems, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Configuration = configuration;
            IntellisenseItems = intellisenseItems;

            PropertyChanged += OnPropertyChanged;

            InstallRootDirectoryPath = Configuration.GetInstallRootDirectoryPath();
        }

        #region property

        private AppConfiguration Configuration { get; }

        private IReadOnlyList<GitHubContentResponseItem> IntellisenseItems { get; }

        public string InstallRootDirectoryPath
        {
            get => this._installRootDirectoryPath;
            set => SetVariable(ref this._installRootDirectoryPath, value);
        }

        public ObservableCollection<IntellisenseDirectoryElement> IntellisenseDirectoryElements { get; } = new ObservableCollection<IntellisenseDirectoryElement>();

        #endregion

        #region function

        private IEnumerable<IntellisenseDirectoryElement> LoadIntellisenseDirectories(string rootDirectoryPath, IEnumerable<string> intellisenseDirectories)
        {
            var dir = IOHelper.GetPhysicalDirectory(rootDirectoryPath);
            if (dir == null)
            {
                Logger.LogWarning($"無効ディレクトリ: {rootDirectoryPath}");
                yield break;
            }

            var result = new List<IntellisenseDirectoryElement>();
            var targetDirectories = intellisenseDirectories.Select(a => Path.Combine(dir.FullName, a));
            foreach (var targetDirectoryPath in targetDirectories)
            {
                var targetDir = new DirectoryInfo(targetDirectoryPath);
                if (targetDir.Exists)
                {
                    yield return new IntellisenseDirectoryElement(targetDir, LoggerFactory);
                }
            }
        }

        #endregion

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(InstallRootDirectoryPath):
                    var elements = LoadIntellisenseDirectories(InstallRootDirectoryPath, Configuration.GetIntellisenseDirectories());
                    IntellisenseDirectoryElements.Clear();
                    foreach (var element in elements)
                    {
                        IntellisenseDirectoryElements.Add(element);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
