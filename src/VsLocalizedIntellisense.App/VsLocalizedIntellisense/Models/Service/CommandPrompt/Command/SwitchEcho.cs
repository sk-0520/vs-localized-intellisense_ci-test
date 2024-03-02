using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt.Command
{
    public class SwitchEcho : Echo
    {
        public SwitchEcho(bool on)
        {
            On = on;
        }

        #region property

        public bool On { get; set; }

        #endregion

        #region Echo

        #endregion
    }
}
