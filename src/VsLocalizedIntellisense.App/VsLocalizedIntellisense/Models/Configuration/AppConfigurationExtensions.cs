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

        private static TResult GetValueOrEmptyInit<TResult>(AppConfiguration configuration, string key)
        {
            return configuration.Contains(key) ? configuration.GetValue<TResult>(key) : default;
        }

        /// <summary>
        /// アップデートチェック用URIを取得。
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Uri GetUpdateCheckUri(this AppConfiguration configuration) => configuration.GetValue<Uri>("update-check-uri");

        public static string GetInstallRootDirectoryPath(this AppConfiguration configuration) => configuration.GetValue<string>("install-root-directory");

        public static LogLevel GetLogDefaultLevel(this AppConfiguration configuration) => configuration.GetValue<LogLevel>("log-default-level");

        public static bool IsEnableDebugLog(this AppConfiguration configuration)
        {
            return GetValueOrEmptyInit<bool>(configuration, "log-debug-is-enabled");
        }
        public static string GetLogDebugFormat(this AppConfiguration configuration)
        {
            return GetValueOrEmptyInit<string>(configuration, "log-debug-format");
        }

        public static bool IsEnableTraceLog(this AppConfiguration configuration)
        {
            return GetValueOrEmptyInit<bool>(configuration, "log-trace-is-enabled");
        }
        public static string GetLogTraceFormat(this AppConfiguration configuration)
        {
            return GetValueOrEmptyInit<string>(configuration, "log-trace-format");
        }

        public static bool IsEnableFileLog(this AppConfiguration configuration)
        {
            return GetValueOrEmptyInit<bool>(configuration, "log-file-is-enabled");
        }
        public static string GetLogFileFormat(this AppConfiguration configuration)
        {
            return GetValueOrEmptyInit<string>(configuration, "log-file-format");
        }
        public static string GetLogFilePath(this AppConfiguration configuration)
        {
            var value = configuration.GetValue<string>("log-file-path");
            return configuration.Replace(value);
        }


        #endregion
    }
}
