using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public abstract class LogOptionsBase
    {
        #region property

        public LogLevel Level { get; }

        #endregion
    }

    public class TraceLogOptions : LogOptionsBase
    { }

    public class FileLogOptions : LogOptionsBase
    {
        #region proeprty

        public string FilePath { get; }

        #endregion
    }

    public class MultiLogOptions : LogOptionsBase
    {
        public IDictionary<string, LogOptionsBase> Options { get; }
    }
}
