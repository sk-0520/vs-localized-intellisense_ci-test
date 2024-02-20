using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Configuration.Internal;
using System.Dynamic;

namespace VsLocalizedIntellisense.Models
{
    /// <summary>
    /// App.config のなんかそれっぽいの。
    /// </summary>
    public sealed class AppConfiguration
    {
        public AppConfiguration()
            : this(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None))
        { }

        public AppConfiguration(Configuration configuration)
        {
            Configuration = configuration;
        }

        #region proeprty

        private Configuration Configuration { get; }

        #endregion

        #region function

        public static AppConfiguration Open(string path)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = path };
            var conf = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return new AppConfiguration(conf);
        }

        public TResult Get<TResult>(string key)
        {
            var setting = Configuration.AppSettings.Settings[key] ?? throw new KeyNotFoundException(key);
            var rawValue = setting.Value;

            var type = typeof(TResult);

            return (TResult)Convert.ChangeType(rawValue, type);
        }

        #endregion
    }
}
