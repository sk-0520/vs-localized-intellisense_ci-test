using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public abstract class LogOptionsBase
    {
        #region property

        /// <summary>
        /// デフォルトログレベル。
        /// <para>このレベル未満はログ出力対象外となる。</para>
        /// </summary>
        public LogLevel Level { get; set; }

        #endregion
    }

    public interface ILogFormatOptions
    {
        #region property

        string Format { get; set; }

        #endregion
    }

    public class DebugLogOptions : LogOptionsBase, ILogFormatOptions
    {
        #region ILogFormatOptions

        public string Format { get; set; } = string.Empty;

        #endregion
    }

    public class FileLogOptions : LogOptionsBase, ILogFormatOptions
    {
        #region proeprty

        public string FilePath { get; set; }

        #endregion

        #region ILogFormatOptions

        public string Format { get; set; } = string.Empty;

        #endregion
    }

    public sealed class MultiLogOptions
    {
        #region property

        public IDictionary<string, LogOptionsBase> Options { get; set; } = new Dictionary<string, LogOptionsBase>();

        #endregion
    }
}
