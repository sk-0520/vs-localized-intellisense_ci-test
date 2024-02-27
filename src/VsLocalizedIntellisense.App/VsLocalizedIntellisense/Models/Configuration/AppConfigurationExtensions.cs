using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static string GetHttpUserAgent(this AppConfiguration configuration)
        {
            var rawHttpUserAgent = configuration.GetValue<string>("http-user-agent");
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var map = new Dictionary<string, string>()
            {
                ["APP:NAME"] = assemblyName.Name,
                ["APP:VERSION"] = assemblyName.Version.ToString(),
            };
            return Strings.ReplaceFromDictionary(rawHttpUserAgent, map);
        }

        /// <summary>
        /// アップデートチェック用URIを取得。
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Uri GetUpdateCheckUri(this AppConfiguration configuration) => configuration.GetValue<Uri>("update-check-uri");

        public static string GetInstallRootDirectoryPath(this AppConfiguration configuration) => configuration.GetValue<string>("install-root-directory");

        public static string[] GetIntellisenseDirectories(this AppConfiguration configuration) => configuration.GetValues<string>("intellisense-directories", '|');

        public static string GetRepositoryRevision(this AppConfiguration configuration) => configuration.GetValue<string>("repository-revision");
        public static string GetRepositoryOwner(this AppConfiguration configuration) => configuration.GetValue<string>("repository-owner");
        public static string GetRepositoryName(this AppConfiguration configuration) => configuration.GetValue<string>("repository-name");

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
