using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt.Command
{
    public class Echo : CommandBase
    {
        public Echo()
            : base(Name)
        { }

        #region property

        public static string Name { get; } = "echo";
        public CommandValue Value { get; set; } = new CommandValue();

        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            if (Value.Arguments.Count == 0 || string.IsNullOrWhiteSpace( ))
            {
                return GetStatementCommandName();
            }

            return $"{GetStatementCommandName()} {Value.ToValue()}";
        }

        #endregion
    }
}
