using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;

namespace VsLocalizedIntellisense.Models.Logger
{
    internal class AppLoggerFactory : ILoggerFactory
    {
        #region variable

        private bool _disposedValue;

        #endregion

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

        // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        ~AppLoggerFactory()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: false);
        }

        #region property

        private MultiLogOptions MultiLogOptions { get; }

        #endregion

        #region function

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                this._disposedValue = true;
            }
        }

        #endregion

        #region ILoggerFactory

        public ILogger CreateLogger(string categoryName)
        {
            return new MultiLogger(categoryName, MultiLogOptions);
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
