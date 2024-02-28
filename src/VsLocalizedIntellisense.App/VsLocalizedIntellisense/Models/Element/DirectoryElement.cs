using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Logger;

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

        public ObservableCollection<LibraryVersionElement> LibraryVersionItems { get; set; }
        public LibraryVersionElement LibraryVersion { get; set; }

        public ObservableCollection<IntellisenseVersionElement> IntellisenseVersionItems { get; }
        public IntellisenseVersionElement IntellisenseVersion { get; set; }

        public ObservableCollection<LanguageElement> LanguageItems { get; }
        public LanguageElement Language { get; set; }

        #endregion
    }
}
