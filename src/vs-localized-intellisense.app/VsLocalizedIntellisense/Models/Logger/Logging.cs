using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;

namespace VsLocalizedIntellisense.Models.Logger
{
    public static class Logging
    {
        #region property

        private static AppLoggerFactory AppLoggerFactory { get; set; }


        internal static ILoggerFactory Instance
        {
            get
            {
                Debug.Assert(AppLoggerFactory != null);
                return AppLoggerFactory;
            }
        }

        #endregion

        #region function

        public static ILoggerFactory Initialize(AppConfiguration configuration)
        {
            AppLoggerFactory = new AppLoggerFactory(configuration);
            return AppLoggerFactory;
        }

        public static bool IsEnabled(LogLevel defaultLevel, LogLevel compareLevel)
        {
            return compareLevel != LogLevel.None && defaultLevel <= compareLevel;
        }

        private static string FormatFromFormatOptions(string category, in LogItem logItem, LogOptionsBase options, ILogFormatOptions formatOptions)
        {
            Debug.Assert(!string.IsNullOrEmpty(formatOptions.Format));

            var map = new Dictionary<string, string>()
            {
                ["TIMESTAMP:LOCAL"] = logItem.Timestamp.ToLocalTime().ToString("u"),
                ["LEVEL"] = logItem.Level.ToString(),
                ["CATEGORY"] = category,
                ["MESSAGE"] = logItem.Message,
                ["FILE"] = logItem.CallerFilePath,
                ["LINE"] = logItem.CallerLineNumber.ToString(),
            };

            var message = Strings.ReplaceFromDictionary(formatOptions.Format, map);
            return message;
        }

        public static string Format(string category, in LogItem logItem, LogOptionsBase options)
        {
            if (options is ILogFormatOptions formatOptions && !string.IsNullOrEmpty(formatOptions.Format))
            {
                return FormatFromFormatOptions(category, logItem, options, formatOptions);
            }

            return logItem.Message;
        }

        #endregion
    }
}
