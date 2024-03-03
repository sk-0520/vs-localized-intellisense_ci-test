using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt
{
    public class CommandPromptEditor
    {
        #region property

        public CommandPromptOptions Options { get; set; } = new CommandPromptOptions();

        public List<ActionBase> Actions { get; } = new List<ActionBase>();

        #endregion
    }
}
