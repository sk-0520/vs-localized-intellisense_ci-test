using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public MainElement(AppConfiguration configuration, IReadOnlyList<string> intellisenseVersionItems, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Configuration = configuration;
            IntellisenseVersionItems = intellisenseVersionItems;

            PropertyChanged += OnPropertyChanged;

            InstallRootDirectoryPath = Configuration.GetInstallRootDirectoryPath();
        }

        #region property

        private AppConfiguration Configuration { get; }

        private IReadOnlyList<string> IntellisenseVersionItems { get; }

        public string InstallRootDirectoryPath
        {
            get => this._installRootDirectoryPath;
            set => SetVariable(ref this._installRootDirectoryPath, value);
        }

        public ObservableCollection<DirectoryElement> IntellisenseDirectoryElements { get; } = new ObservableCollection<DirectoryElement>();

        #endregion

        #region function

        private IEnumerable<DirectoryElement> LoadIntellisenseDirectories(string rootDirectoryPath, IEnumerable<string> intellisenseDirectories, IEnumerable<string> intellisenseVersionItems)
        {
            var dir = IOHelper.GetPhysicalDirectory(rootDirectoryPath);
            if (dir == null)
            {
                Logger.LogWarning($"無効ディレクトリ: {rootDirectoryPath}");
                yield break;
            }

            var result = new List<DirectoryElement>();
            var targetDirectories = intellisenseDirectories.Select(a => Path.Combine(dir.FullName, a));
            foreach (var targetDirectoryPath in targetDirectories)
            {
                var targetDir = new DirectoryInfo(targetDirectoryPath);
                if (targetDir.Exists)
                {
                    var libraryVersionItems = targetDir.GetDirectories().Select(a => new LibraryVersionElement(a.Name, LoggerFactory)).OrderBy(a => a.Version).ToList();

                    Regex regex = null;
                    if (Configuration.GetIntellisenseDotNetStandardMappings().Any(a => a == targetDir.Name))
                    {
                        regex = Configuration.GetIntellisenseDotNetStandardVersion();
                    }
                    else if (Configuration.GetIntellisenseDotNetRuntimeMappings().Any(a => a == targetDir.Name))
                    {
                        regex = Configuration.GetIntellisenseDotNetRuntimeVersion();
                    }
                    if (regex == null)
                    {
                        Logger.LogWarning($"{targetDir.Name}");
                        continue;
                    }

                    var intellisenseVersions = new List<IntellisenseVersionElement>();
                    foreach (var s in intellisenseVersionItems)
                    {
                        var match = regex.Match(s);
                        if (!match.Success)
                        {
                            continue;
                        }
                        var intellisenseName = match.Groups["NAME"].Value;
                        var rawIntellisenseVersion = match.Groups["VERSION"].Value;
                        var intellisenseVersion = new Version(rawIntellisenseVersion);
                        intellisenseVersions.Add(new IntellisenseVersionElement(intellisenseName, intellisenseVersion, LoggerFactory));
                    }
                    intellisenseVersions = intellisenseVersions.OrderBy(a => a.Version).ToList();

                    yield return new DirectoryElement(targetDir, libraryVersionItems, libraryVersionItems.Last(), intellisenseVersions, intellisenseVersions.Last(), LoggerFactory);
                }
            }
        }

        #endregion

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(InstallRootDirectoryPath):
                    var elements = LoadIntellisenseDirectories(InstallRootDirectoryPath, Configuration.GetIntellisenseDirectories(), IntellisenseVersionItems);
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
