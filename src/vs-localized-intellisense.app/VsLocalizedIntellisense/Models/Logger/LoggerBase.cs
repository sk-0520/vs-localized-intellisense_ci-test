using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public abstract class LoggerBase : ILogger
    {
        #region property

        /// <summary>
        /// ロガーの対象ログレベル。
        /// </summary>
        protected LogLevel CurrentLogLevel { get; }

        #endregion

        #region ILogger

        public bool IsEnabled(LogLevel logLevel)
        {
            return CurrentLogLevel <= logLevel;
        }

        public abstract void Log(LogLevel logLevel, string logMessage, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        #endregion
    }
}
