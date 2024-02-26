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
        public IntellisenseVersionElement(string version, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Version = version;
        }

        #region proeprty

        public string Version { get; }

        #endregion
    }
}
