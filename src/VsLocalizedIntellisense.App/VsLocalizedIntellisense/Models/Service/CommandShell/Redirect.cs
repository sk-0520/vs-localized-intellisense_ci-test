using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Service.CommandShell.Value;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    /// <summary>
    /// リダイレクト基底。
    /// </summary>
    public abstract class RedirectBase : IExpression
    {
        #region property

        /// <summary>
        /// リダイレクト先。
        /// </summary>
        public Express Target { get; set; }
        /// <summary>
        /// 追記するか。
        /// </summary>
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

                var sb = new StringBuilder();

                var redirectMode = Append
                    ? ">>"
                    : ">"
                ;
                sb.Append(redirectMode);
                sb.Append(' ');
                sb.Append(Target.Expression);

                return sb.ToString();
            }
        }

        #endregion
    }

    /// <summary>
    /// ファイル出力。
    /// </summary>
    public class Redirect : RedirectBase
    {
        #region property

        /// <summary>
        /// 空リダイレクト。
        /// </summary>
        public static Redirect Null => new Redirect()
        {
            Target = "NUL",
        };

        /// <summary>
        /// 空リダイレクト(エラー付き)。
        /// </summary>
        public static Redirect NullWithError => new Redirect()
        {
            Target = "NUL",
            Error = new ErrorRedirect()
            {
                StandardOutput = true,
            }
        };

        /// <summary>
        /// エラー出力先。
        /// </summary>
        public ErrorRedirect Error { get; set; }

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

    /// <summary>
    /// 標準エラー出力。
    /// </summary>
    public class ErrorRedirect : RedirectBase
    {
        #region property

        /// <summary>
        /// 標準出力に追記する。
        /// </summary>
        public bool StandardOutput { get; set; }

        #endregion
    }
}
