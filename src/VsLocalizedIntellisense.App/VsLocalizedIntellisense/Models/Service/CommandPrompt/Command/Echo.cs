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

        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            if (Value.Arguments.Count == 0)
            {
                return GetStatementCommandName();
            }

            return $"{GetStatementCommandName()} {Value.ToValue()}";
        }

        #endregion
    }
}
