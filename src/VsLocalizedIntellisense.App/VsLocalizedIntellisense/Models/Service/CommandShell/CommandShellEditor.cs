using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public class CommandShellEditor
    {
        #region property

        public CommandShellOptions Options { get; set; } = new CommandShellOptions();

        public List<ActionBase> Actions { get; } = new List<ActionBase>();

        #endregion
    }
}
