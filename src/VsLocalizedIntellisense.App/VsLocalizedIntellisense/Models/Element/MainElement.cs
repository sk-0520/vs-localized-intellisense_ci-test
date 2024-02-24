using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.Models.Element
{
    public class MainElement : ElementBase
    {
        public MainElement(AppConfiguration configuration, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Configuration = configuration;

            InstallRootDirectoryPath = Configuration.GetInstallRootDirectoryPath();
        }

        #region property

        public AppConfiguration Configuration { get; }

        public string InstallRootDirectoryPath { get; set; }

        #endregion

        #region function

        public DirectoryInfo GetInstallRootDirectory()
        {
            if (string.IsNullOrWhiteSpace(InstallRootDirectoryPath))
            {
                return null;
            }

            var dir = Environment.ExpandEnvironmentVariables(InstallRootDirectoryPath);
            if (!Directory.Exists(dir))
            {
                return null;
            }

            return new DirectoryInfo(dir);
        }

        #endregion
    }
}
