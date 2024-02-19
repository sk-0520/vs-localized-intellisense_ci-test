using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    internal sealed class MultiLogger : LoggerBase
    {
        public MultiLogger(MultiLogOptions multiLogOptions)
            : base(multiLogOptions)
        {
            var loggers = new List<LoggerBase>();

            foreach (var currentOptions in multiLogOptions.Options.Values)
            {
                if (currentOptions is TraceLogOptions)
                {
                    var options = (TraceLogOptions)currentOptions;
                    var logger = new TraceLogger(options);
                    loggers.Add(logger);
                }
                else if (currentOptions is FileLogOptions)
                {
                    var options = (FileLogOptions)currentOptions;
                    var logger = new FileLogger(options);
                    loggers.Add(logger);
                }
            }

            Loggers = loggers;
        }

        #region property

        private IReadOnlyCollection<ILogger> Loggers { get; }

        #endregion

        #region LoggerBase

        protected override void LogImpl(LogLevel logLevel, string logMessage, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            foreach (var logger in Loggers)
            {
                logger.Log(logLevel, logMessage, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        #endregion
    }
}
