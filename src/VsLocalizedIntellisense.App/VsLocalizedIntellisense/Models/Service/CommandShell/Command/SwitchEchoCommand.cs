using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandShell.Command
{
    public class SwitchEchoCommand : EchoCommand
    {
        public SwitchEchoCommand()
            : base()
        {
            SuppressCommand = true;
        }

        #region property

        public bool On { get; set; }

        #endregion

        #region Echo

        public override string Value {
            get => On ? "on" : "off";
            set => throw new NotSupportedException();
        }

        #endregion
    }
}
