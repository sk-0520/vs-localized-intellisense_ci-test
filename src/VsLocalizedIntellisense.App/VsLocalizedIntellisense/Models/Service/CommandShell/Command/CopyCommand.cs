using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Service.CommandShell.Value;

namespace VsLocalizedIntellisense.Models.Service.CommandShell.Command
{
    public class CopyCommand : CommandBase
    {
        public CopyCommand()
            : base(Name)
        { }

        #region property

        public static string Name { get; } = "copy";

        /// <summary>
        /// 暗号化されたファイルをコピー先で暗号化解除されたファイルとして保存できるようにします。
        /// <para>/d</para>
        /// </summary>
        public bool IsDecryption { get; set; }
        /// <summary>
        /// 新しいファイルが正しく書き込まれたことを確認します。
        /// <para>/v</para>
        /// </summary>
        public bool IsVerify { get; set; }
        /// <summary>
        /// プロンプト状態。
        /// <para>[<see langword="true"/>] /y: 既存の宛先ファイルを上書きするかどうかを確認するプロンプトを抑制します。</para>
        /// <para>[<see langword="false"/>] /-y: 既存のリンク先ファイルを上書きするかどうかを確認するプロンプトを表示します。</para>
        /// <para>[<see langword="null"/>] デフォルト。</para>
        /// </summary>
        public bool? IsForce { get; set; }

        public Express Source { get; set; }
        public Express Destination { get; set; }

        #endregion

        #region function
        #endregion

        #region CommandBase

        public override string GetStatement()
        {
            var src = Source?.Expression;
            var dst = Destination?.Expression;
            if (string.IsNullOrWhiteSpace(src))
            {
                throw new InvalidOperationException(nameof(Source));
            }
            if (string.IsNullOrWhiteSpace(dst))
            {
                throw new InvalidOperationException(nameof(Destination));
            }

            var sb = new StringBuilder();

            sb.Append(GetStatementCommandName());
            sb.Append(' ');

            if (IsDecryption)
            {
                sb.Append("/d ");
            }

            if (IsVerify)
            {
                sb.Append("/v ");
            }

            if (IsForce.HasValue)
            {
                if (IsForce.Value)
                {
                    sb.Append("/y ");
                }
                else
                {
                    sb.Append("/-y ");
                }
            }

            sb.Append(src);
            sb.Append(' ');
            sb.Append(dst);

            return sb.ToString();
        }

        #endregion
    }
}
