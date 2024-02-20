using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public sealed class DebugLogger : OutputLoggerBase<DebugLogOptions>
    {
        public DebugLogger(DebugLogOptions options)
            : base(options)
        { }

        #region OutputLoggerBase

        protected internal override void OutputLog(in LogItem logItem)
        {
            var log = Logging.Format(logItem, Options);
            Debug.WriteLine(log);
        }

        #endregion
    }
}
