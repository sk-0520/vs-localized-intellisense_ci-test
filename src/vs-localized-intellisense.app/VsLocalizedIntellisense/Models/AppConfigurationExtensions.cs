using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models
{
    public static class AppConfigurationExtensions
    {
        #region function

        /// <summary>
        /// アップデートチェック用URIを取得。
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Uri GetUpdateCheckUri(this AppConfiguration configuration) => configuration.GetValue<Uri>("update-check-uri");

        #endregion
    }
}
