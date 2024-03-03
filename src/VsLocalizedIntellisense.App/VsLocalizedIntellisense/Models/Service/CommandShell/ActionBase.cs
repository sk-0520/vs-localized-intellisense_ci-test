using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public abstract class ActionBase
    {
        #region property

        public virtual string CommandName { get; set; }
        public Value Input { get; set; }
        public Redirect Redirect { get; set; }
        public ActionBase Pipe { get; set; }

        #endregion

        #region function

        public abstract string GetStatement();

        public virtual string ToStatement(IndentContext indent)
        {
            var sb = new StringBuilder();

            var statement = GetStatement();
            sb.Append(statement);

            if (Input != null)
            {
                sb.Append(" < ");
                sb.Append(Input.Expression);
            }

            if (Pipe != null)
            {
                var pipeAction = Pipe.ToStatement(new IndentContext());
                sb.Append(" | ");
                sb.Append(pipeAction);
            }
            else if (Redirect != null)
            {
                var redirect = Redirect.Expression;
                if (!string.IsNullOrWhiteSpace(redirect))
                {
                    sb.Append(' ');
                    sb.Append(redirect);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
