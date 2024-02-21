using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    internal sealed class MultiLogger : ILogger
    {
        public MultiLogger(string category, MultiLogOptions multiLogOptions)
        {
            var loggers = new List<LoggerBase>();

            foreach (var currentOptions in multiLogOptions.Options.Values)
            {
                switch (currentOptions)
                {
                    case DebugLogOptions options:
                        {
                            var logger = new DebugLogger(category, options);
                            loggers.Add(logger);
                        }
                        break;

                    case TraceLogOptions options:
                        {
                            var logger = new TraceLogger(category, options);
                            loggers.Add(logger);
                        }
                        break;

                    case FileLogOptions options:
                        {
                            var logger = new FileLogger(category, options);
                            loggers.Add(logger);
                        }
                        break;
                }
            }

            Loggers = loggers;
        }

        #region property

        private IReadOnlyCollection<LoggerBase> Loggers { get; }

        #endregion

        #region ILogger

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, string logMessage, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            var logItem = new LogItem(DateTime.UtcNow, logLevel, logMessage, callerMemberName, callerFilePath, callerLineNumber);

            foreach (var logger in Loggers)
            {
                if (logger.IsEnabled(logLevel))
                {
                    logger.OutputLog(logItem);
                }
            }
        }

        #endregion
    }
}
