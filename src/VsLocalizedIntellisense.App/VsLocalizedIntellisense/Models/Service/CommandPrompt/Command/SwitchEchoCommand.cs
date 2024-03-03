using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt.Command
{
    public class SwitchEchoCommand : EchoCommand
    {
        public SwitchEchoCommand(bool on)
        {
            SuppressCommand = true;
            On = on;
        }

        #region property

        public bool On { get; set; }

        #endregion

        #region Echo

        public override string GetStatement()
        {
            var value = On ? "on" : "off";
            return $"{GetStatementCommandName()} {value}";
        }

        #endregion
    }
}
