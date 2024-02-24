using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models;

namespace VsLocalizedIntellisense.Test.Models
{
    [TestClass]
    public class IOHelperTest
    {
#if !CUSTOM_ENV
        static readonly string EnvKeySystemRoot = "%SystemRoot%";
        static readonly string EnvKeyTemp = "%TEMP%";
        static readonly string EnvKeyUnknown = "%?%";
#else
        static readonly string EnvKeySystemRoot = "%CUSTOM_SystemRoot%";
        static readonly string EnvKeyTemp = "%CUSTOM_TEMP%";
        static readonly string EnvKeyUnknown = "%CUSTOM_?%";
#endif


        static readonly string EnvValueSystemRoot = Environment.ExpandEnvironmentVariables(EnvKeySystemRoot);
        static readonly string EnvValueTemp = Environment.ExpandEnvironmentVariables(EnvKeyTemp);

        #region function

        [TestMethod]
        public void GetPhysicalDirectory_SystemRoot_Test()
        {
            var actual = IOHelper.GetPhysicalDirectory(EnvKeySystemRoot);
            Assert.AreEqual(new DirectoryInfo(EnvValueSystemRoot).FullName, actual.FullName);
        }


        [TestMethod]
        public void GetPhysicalDirectory_Temp_Test()
        {
            var actual = IOHelper.GetPhysicalDirectory(EnvKeyTemp);
            Assert.AreEqual(new DirectoryInfo(EnvValueTemp).FullName, actual.FullName);
        }

        [TestMethod]
        public void GetPhysicalDirectory_Unknown_Test()
        {
            var actual = IOHelper.GetPhysicalDirectory(EnvKeyUnknown);
            Assert.IsNull(actual);
        }


        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void GetPhysicalDirectory_Empty_Test(string input)
        {
            var actual = IOHelper.GetPhysicalDirectory(input);
            Assert.IsNull(actual);
        }

        #endregion
    }
}
