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
    public class IntellisenseDirectoryElement : ElementBase
    {
        public IntellisenseDirectoryElement(DirectoryInfo directory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Directory = directory;
            VersionItems = new ObservableCollection<IntellisenseVersionElement>(Directory.GetDirectories().Select(a => new IntellisenseVersionElement(a.Name, LoggerFactory)).OrderBy(a => a.Version));
            SelectedVersion = VersionItems.Last();
        }

        #region property

        public DirectoryInfo Directory { get; }

        public ObservableCollection<IntellisenseVersionElement> VersionItems { get; set; }

        public IntellisenseVersionElement SelectedVersion { get; set; }

        #endregion
    }
}
