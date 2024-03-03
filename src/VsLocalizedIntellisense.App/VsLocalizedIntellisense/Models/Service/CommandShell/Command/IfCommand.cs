using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Service.CommandShell.Value;
using VsLocalizedIntellisense.Models.Service.CommandShell.Value.Special;

namespace VsLocalizedIntellisense.Models.Service.CommandShell.Command
{
    public abstract class IfCommandBase : CommandBase
    {
        public IfCommandBase()
            : base(Name)
        { }

        #region property

        public static string Name { get; } = "if";
        public static string ElseName { get; } = "else";

        protected bool IsNot { get; set; }

        protected virtual Express Condition { get; }

        public IList<CommandBase> TrueBlock { get; } = new List<CommandBase>();
        public IList<CommandBase> FalseBlock { get; } = new List<CommandBase>();

        public string IndentSpace { get; set; } = "\t";

        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            var sb = new StringBuilder();

            sb.Append(GetStatementCommandName());
            sb.Append(' ');
            if (IsNot)
            {
                sb.Append("not ");
            }
            sb.Append(Condition.Expression);
            sb.AppendLine(" (");
            foreach (var command in TrueBlock)
            {
                sb.Append(IndentSpace);
                sb.AppendLine(command.GetStatement());
            }
            if (FalseBlock.Any())
            {
                sb.Append(") ");

                sb.Append(ElseName);
                sb.AppendLine(" (");
                foreach (var command in FalseBlock)
                {
                    sb.Append(IndentSpace);
                    sb.AppendLine(command.GetStatement());
                }
                sb.AppendLine(")");
            }
            else
            {
                sb.AppendLine(")");
            }

            return sb.ToString();
        }

        #endregion
    }

    public class IfErrorLevelCommand : IfCommandBase
    {
        #region property

        public int Level { get; set; }

        #endregion

        #region IfCommandBase

        protected override Express Condition
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(ErrorLevel.Instance.Name);
                sb.Append(' ');
                sb.Append(Level);

                return sb.ToString();
            }
        }

        #endregion
    }

}
