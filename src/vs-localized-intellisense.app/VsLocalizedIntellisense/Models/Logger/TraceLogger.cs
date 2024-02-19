using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public sealed class TraceLogger : LoggerBase<TraceLogOptions>
    {
        public TraceLogger(TraceLogOptions options)
            : base(options)
        { }

        #region LoggerBase

        protected internal override void LogImpl(DateTime utcTimestamp, LogLevel logLevel, string logMessage, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Trace.WriteLine(logMessage);
                    break;

                case LogLevel.Debug:
                    Trace.WriteLine(logMessage);
                    break;

                case LogLevel.Information:
                    Trace.WriteLine(logMessage);
                    break;

                case LogLevel.Warning:
                    Trace.WriteLine(logMessage);
                    break;

                case LogLevel.Error:
                    Trace.WriteLine(logMessage);
                    break;

                case LogLevel.Critical:
                    Trace.WriteLine(logMessage);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
