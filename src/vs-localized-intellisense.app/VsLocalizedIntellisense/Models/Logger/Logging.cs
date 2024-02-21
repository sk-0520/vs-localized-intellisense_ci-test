using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public static class Logging
    {
        #region property

        private static MultiLogger Logger { get; set; }


        internal static ILogger Instance
        {
            get
            {
                Debug.Assert(Logger != null);
                return Logger;
            }
        }

        #endregion

        #region function

        public static void Initialize()
        {
        }

        public static bool IsEnabled(LogLevel defaultLevel, LogLevel compareLevel)
        {
            return compareLevel != LogLevel.None && defaultLevel <= compareLevel;
        }

        private static string FormatFromFormatOptions(in LogItem logItem, LogOptionsBase options, ILogFormatOptions formatOptions)
        {
            Debug.Assert(!string.IsNullOrEmpty(formatOptions.Format));

            return logItem.Message;
        }

        public static string Format(in LogItem logItem, LogOptionsBase options)
        {
            if (options is ILogFormatOptions formatOptions && !string.IsNullOrEmpty(formatOptions.Format))
            {
                return FormatFromFormatOptions(logItem, options, formatOptions);
            }

            return logItem.Message;
        }

        #endregion
    }
}
