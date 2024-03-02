using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Data;
using VsLocalizedIntellisense.Models.Logger;
using VsLocalizedIntellisense.Models.Service.Application;
using VsLocalizedIntellisense.Models.Service.GitHub;

namespace VsLocalizedIntellisense.Models.Element
{
    public class DirectoryElement : ElementBase
    {
        public DirectoryElement(DirectoryInfo directory, IEnumerable<LibraryVersionElement> libraryVersionItems, LibraryVersionElement libraryVersion, IEnumerable<IntellisenseVersionElement> intellisenseVersions, IntellisenseVersionElement intellisenseVersion, IEnumerable<LanguageElement> languageItems, LanguageElement language, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Directory = directory;
            LibraryVersionItems = new ObservableCollection<LibraryVersionElement>(libraryVersionItems);
            LibraryVersion = libraryVersion;

            IntellisenseVersionItems = new ObservableCollection<IntellisenseVersionElement>(intellisenseVersions);
            IntellisenseVersion = intellisenseVersion;

            LanguageItems = new ObservableCollection<LanguageElement>(languageItems);
            Language = language;
        }

        #region property

        public DirectoryInfo Directory { get; }

        /// <summary>
        /// ダウンロード対象か。
        /// </summary>
        public bool IsDownloadTarget { get; set; } = true;

        public ObservableCollection<LibraryVersionElement> LibraryVersionItems { get; set; }
        public LibraryVersionElement LibraryVersion { get; set; }

        public ObservableCollection<IntellisenseVersionElement> IntellisenseVersionItems { get; }
        public IntellisenseVersionElement IntellisenseVersion { get; set; }

        public ObservableCollection<LanguageElement> LanguageItems { get; }
        public LanguageElement Language { get; set; }

        #endregion

        #region function

        public async Task<FileInfo[]> DownloadIntellisenseFilesAsync(DirectoryInfo downloadDirectory, AppFileService fileService, GitHubService gitHubService, CancellationToken cancellationToken = default)
        {
            var languageParts = new IntellisenseLanguageParts()
            {
                IntellisenseVersion = IntellisenseVersion.DirectoryName,
                LibraryName = Directory.Name,
                Language = Language.Language,
            };
            var languageData = fileService.GetIntellisenseLanguageData(languageParts);
            if (languageData != null)
            {
                Logger.LogInformation($"キャッシュからインテリセンス言語データ取得: {languageParts}");
            }
            else
            {
                Logger.LogInformation($"GitHubからインテリセンス言語データ取得: {languageParts}");
                var languageItems = await gitHubService.GetIntellisenseLanguageItems(languageParts, cancellationToken);

                languageData = new IntellisenseLanguageData();
                languageData.LanguageItems = languageItems.ToArray();
                fileService.SaveIntellisenseLanguageData(languageParts, languageData);
            }

            return Array.Empty<FileInfo>();
        }

        #endregion
    }
}
