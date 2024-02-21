using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Configuration
{
    public class AppConfigurationInitializeParameters
    {
        public AppConfigurationInitializeParameters(DateTime utcTimestamp)
        {
            UtcTimestamp = utcTimestamp;
        }

        #region proeprty

        public DateTime UtcTimestamp { get; }

        #endregion
    }
}
