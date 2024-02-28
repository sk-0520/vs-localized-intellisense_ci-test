using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Data
{
    public class IntellisenseVersionData: ICachedTimestamp
    {
        #region property

        public string[] VersionItems {get;set;} = Array.Empty<string>();

        #endregion

        #region ICachedTimestamp

        public DateTimeOffset CachedTimestamp { get; set; } = DateTimeOffset.Now;

        #endregion
    }
}
