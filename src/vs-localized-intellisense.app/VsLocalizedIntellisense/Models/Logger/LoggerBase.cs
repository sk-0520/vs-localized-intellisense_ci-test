using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VsLocalizedIntellisense.Models.Logger
{
    public abstract class LoggerBase : ILogger
    {
        protected LoggerBase(LogOptionsBase options)
        {
            CurrentLogLevel = options.Level;
        }

        #region property

        /// <summary>
        /// ロガーの対象ログレベル。
        /// </summary>
        protected LogLevel CurrentLogLevel { get; }

        #endregion

        #region function

        protected internal abstract void LogImpl(DateTime utcTimestamp, LogLevel logLevel, string logMessage, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        #endregion

        #region ILogger

        public bool IsEnabled(LogLevel logLevel)
        {
            return CurrentLogLevel <= logLevel;
        }

        public void Log(LogLevel logLevel, string logMessage, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            LogImpl(DateTime.UtcNow, logLevel, logMessage, callerMemberName, callerFilePath, callerLineNumber);
        }


        #endregion
    }

    public abstract class LoggerBase<TLogOptions>: LoggerBase
        where TLogOptions: LogOptionsBase
    {
        protected LoggerBase(TLogOptions options)
            : base(options)
        {
            Options = options;
        }

        #region property

        protected TLogOptions Options { get; }

        #endregion  
    }
}
