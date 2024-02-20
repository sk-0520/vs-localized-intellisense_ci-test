using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public sealed class FileLogger : OutputLoggerBase<FileLogOptions>, IDisposable
    {
        #region variable

        private bool _disposedValue;

        #endregion  

        public FileLogger(FileLogOptions options)
            : base(options)
        {
            var stream = File.Exists(Options.FilePath)
                ? new FileStream(Options.FilePath, FileMode.Append, FileAccess.ReadWrite, FileShare.Read)
                : new FileStream(Options.FilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read)
            ;

            Writer = new StreamWriter(stream);
        }

        ~FileLogger()
        {
            Dispose(false);
        }

        #region proeprty

        private TextWriter Writer { get; }

        #endregion

        #region LoggerBase

        protected internal override void OutputLog(in LogItem logItem)
        {
            var log = Logging.Format(logItem, Options);
            Writer.WriteLine(log);
        }

        #endregion

        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    Writer.Dispose();
                }

                this._disposedValue = true;
            }
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
