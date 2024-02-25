using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;

namespace VsLocalizedIntellisense.Models.Logger
{
    internal class AppLoggerFactory : DisposerBase, ILoggerFactory
    {
        public AppLoggerFactory(AppConfiguration configuration)
        {
            var multiLogOptions = new MultiLogOptions();

            var level = configuration.GetLogDefaultLevel();

            if (configuration.IsEnableDebugLog())
            {
                var options = new DebugLogOptions()
                {
                    Level = level,
                    Format = configuration.GetLogDebugFormat(),
                };

                multiLogOptions.Options[nameof(DebugLogOptions)] = options;
            }

            if (configuration.IsEnableTraceLog())
            {
                var options = new TraceLogOptions()
                {
                    Level = level,
                    Format = configuration.GetLogTraceFormat(),
                };

                multiLogOptions.Options[nameof(TraceLogOptions)] = options;
            }

            if (configuration.IsEnableFileLog())
            {
                var options = new FileLogOptions()
                {
                    Level = level,
                    Format = configuration.GetLogFileFormat(),
                    FilePath = configuration.GetLogFilePath(),
                };

                multiLogOptions.Options[nameof(FileLogOptions)] = options;
            }

            MultiLogOptions = multiLogOptions;
        }

        #region property

        private MultiLogOptions MultiLogOptions { get; }

        #endregion

        #region function

        #endregion

        #region ILoggerFactory

        public ILogger CreateLogger(string categoryName)
        {
            return new MultiLogger(categoryName, MultiLogOptions);
        }

        #endregion
    }
}
