using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.Models.Configuration
{
    public static class AppConfigurationExtensions
    {
        #region function

        /// <summary>
        /// アップデートチェック用URIを取得。
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Uri GetUpdateCheckUri(this AppConfiguration configuration) => configuration.GetValue<Uri>("update-check-uri");

        public static LogLevel GetLogDefaultLevel(this AppConfiguration configuration) => configuration.GetValue<LogLevel>("log-default-level");

        public static bool IsEnableDebugLog(this AppConfiguration configuration)
        {
            var key = "log-debug-is-enabled";
            return configuration.Contains(key) && configuration.GetValue<bool>(key);
        }
        public static string GetLogDebugFormat(this AppConfiguration configuration)
        {
            var key = "log-debug-format";
            return configuration.Contains(key) ? configuration.GetValue<string>(key): string.Empty;
        }

        public static bool IsEnableTraceLog(this AppConfiguration configuration)
        {
            var key = "log-trace-is-enabled";
            return configuration.Contains(key) && configuration.GetValue<bool>(key);
        }
        public static string GetLogTraceFormat(this AppConfiguration configuration)
        {
            var key = "log-trace-format";
            return configuration.Contains(key) ? configuration.GetValue<string>(key) : string.Empty;
        }

        public static bool IsEnableFileLog(this AppConfiguration configuration)
        {
            var key = "log-file-is-enabled";
            return configuration.Contains(key) && configuration.GetValue<bool>(key);
        }
        public static string GetLogFileFormat(this AppConfiguration configuration)
        {
            var key = "log-file-format";
            return configuration.Contains(key) ? configuration.GetValue<string>(key) : string.Empty;
        }
        public static string GetLogFilePath(this AppConfiguration configuration)
        {
            var value = configuration.GetValue<string>("log-file-path");
            return configuration.Replace(value);
        }


        #endregion
    }
}
