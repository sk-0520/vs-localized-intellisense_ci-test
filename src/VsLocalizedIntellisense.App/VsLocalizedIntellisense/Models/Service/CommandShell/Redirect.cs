using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public abstract class RedirectBase : IExpression
    {
        #region property

        public Value Target { get; set; }
        public bool Append { get; set; }

        #endregion

        #region IExpression

        public virtual string Expression
        {
            get
            {
                if (Target == null)
                {
                    return string.Empty;
                }

                var redirectMode = Append
                    ? ">>"
                    : ">"
                ;
                return $"{redirectMode} {Target.Expression}";
            }
        }

        #endregion
    }

    public class Redirect : RedirectBase
    {
        #region property

        public static Redirect Null => new Redirect()
        {
            Target = "NUL",
        };

        public virtual ErrorRedirect Error { get; set; }

        #endregion

        #region IExpression

        public override string Expression
        {
            get
            {
                var sb = new StringBuilder();

                var expression = base.Expression;
                var hasStdOut = !string.IsNullOrWhiteSpace(expression);
                if (hasStdOut)
                {
                    sb.Append(expression);
                }

                if (Error != null)
                {
                    if (Error.StandardOutput)
                    {
                        if (hasStdOut)
                        {
                            sb.Append(' ');
                        }
                        sb.Append("2>&1");
                    }
                    else
                    {
                        if (hasStdOut)
                        {
                            sb.Append(' ');
                        }

                        var redirect = Error.Expression;
                        if (!string.IsNullOrWhiteSpace(redirect))
                        {
                            sb.Append('2');
                            sb.Append(redirect);
                        }
                    }
                }

                return sb.ToString();
            }
        }

        #endregion
    }

    public class ErrorRedirect : RedirectBase
    {
        #region property

        public bool StandardOutput { get; set; }

        #endregion
    }
}
