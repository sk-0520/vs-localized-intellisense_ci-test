using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Service.CommandShell.Value;

namespace VsLocalizedIntellisense.Models.Service.CommandShell.Command
{
    public class RemarkCommand: CommandBase
    {
        public RemarkCommand()
            : base(Name)
        { }

        #region property

        public static string Name { get; } = "rem";

        public virtual Express Value { get; set; } = string.Empty;

        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            var value = Value.Expression;
            if (string.IsNullOrEmpty(value))
            {
                return $"{GetStatementCommandName()}";
            }

            return $"{GetStatementCommandName()} {value}";
        }

        #endregion
    }
}
