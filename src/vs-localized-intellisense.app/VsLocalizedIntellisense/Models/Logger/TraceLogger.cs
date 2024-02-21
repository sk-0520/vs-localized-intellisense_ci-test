using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public sealed class TraceLogger : OutputLoggerBase<TraceLogOptions>
    {
        public TraceLogger(string category, TraceLogOptions options)
            : base(category, options)
        { }

        #region OutputLoggerBase

        protected internal override void OutputLog(in LogItem logItem)
        {
            var log = Logging.Format(Category, logItem, Options);
            Trace.WriteLine(log);
        }

        #endregion
    }
}
