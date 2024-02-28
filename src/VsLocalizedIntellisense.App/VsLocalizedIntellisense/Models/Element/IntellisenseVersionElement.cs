using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.Models.Element
{
    public class IntellisenseVersionElement : ElementBase
    {
        public IntellisenseVersionElement(string rawVersion, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            RawVersion = rawVersion;
            Version = new Version(RawVersion);
        }

        #region proeprty

        private string RawVersion { get; }

        public Version Version { get; }

        #endregion
    }
}
