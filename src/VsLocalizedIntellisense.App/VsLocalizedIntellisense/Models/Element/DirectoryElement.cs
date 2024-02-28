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
        public DirectoryElement(DirectoryInfo directory, IEnumerable<LibraryVersionElement> libraryVersionItems, LibraryVersionElement libraryVersion, IEnumerable<IntellisenseVersionElement> intellisenseVersions, IntellisenseVersionElement intellisenseVersion, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Directory = directory;
            LibraryVersionItems = new ObservableCollection<LibraryVersionElement>(libraryVersionItems);
            LibraryVersion = libraryVersion;

            IntellisenseVersionItems = new ObservableCollection<IntellisenseVersionElement>(intellisenseVersions);
            IntellisenseVersion = intellisenseVersion;
        }

        #region property

        public DirectoryInfo Directory { get; }

        public ObservableCollection<LibraryVersionElement> LibraryVersionItems { get; set; }

        public LibraryVersionElement LibraryVersion { get; set; }

        public ObservableCollection<IntellisenseVersionElement> IntellisenseVersionItems { get; }
        public IntellisenseVersionElement IntellisenseVersion { get; set; }

        #endregion
    }
}
