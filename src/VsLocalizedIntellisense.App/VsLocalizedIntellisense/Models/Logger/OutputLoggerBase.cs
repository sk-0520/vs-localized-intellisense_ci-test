using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public abstract class OutputLoggerBase<TLogOptions> : LoggerBase<TLogOptions>
        where TLogOptions : LogOptionsBase
    {
        protected OutputLoggerBase(string category, TLogOptions options)
            : base(category, options)
        { }
    }
}
