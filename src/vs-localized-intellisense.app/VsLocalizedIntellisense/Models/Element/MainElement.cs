using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.Models.Element
{
    public class MainElement : ElementBase
    {
        public MainElement(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }
    }
}
