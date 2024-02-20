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
    public class AppConfigurationTest
    {
        #region function

        private AppConfiguration GetAppConfiguration()
        {
            var path = Path.Combine(Test.GetProjectDirectory().FullName, "Models", "AppConfigurationTest.config");
            return AppConfiguration.Open(path);
        }

        [TestMethod]
        public void Get_notFound_Test()
        {
            var config = GetAppConfiguration();
            Assert.ThrowsException<KeyNotFoundException>(() => config.Get<object>(""));
        }

        [TestMethod]
        [DataRow("TEXT", "string")]
        [DataRow("2147483647", "int")]
        [DataRow("9223372036854775807", "long")]
        [DataRow("3.14", "float")]
        [DataRow("2.71", "double")]
        [DataRow("0.1", "decimal")]
        public void Get_string_Test(string expected, string key)
        {
            var config = GetAppConfiguration();
            var actual = config.Get<string>(key);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Get_int_Test()
        {
            var config = GetAppConfiguration();
            var actual = config.Get<int>("int");
            Assert.AreEqual(2147483647, actual);
        }

        [TestMethod]
        [DataRow("string")]
        [DataRow("float")]
        [DataRow("double")]
        [DataRow("decimal")]
        public void Get_int_FormatException_Test(string key)
        {
            var config = GetAppConfiguration();
            Assert.ThrowsException<FormatException>(() => config.Get<int>(key));
        }

        public void Get_int_OverflowException_Test()
        {
            var config = GetAppConfiguration();
            Assert.ThrowsException<OverflowException>(() => config.Get<int>("long"));
        }

        [TestMethod]
        public void Get_long_Test()
        {
            var config = GetAppConfiguration();
            var actualLong = config.Get<long>("long");
            Assert.AreEqual(9223372036854775807, actualLong);
            var actualInt = config.Get<long>("int");
            Assert.AreEqual(2147483647, actualInt);
        }

        [TestMethod]
        [DataRow("string")]
        [DataRow("float")]
        [DataRow("double")]
        [DataRow("decimal")]
        public void Get_long_throw_Test(string key)
        {
            var config = GetAppConfiguration();
            Assert.ThrowsException<FormatException>(() => config.Get<long>(key));
        }

        #endregion
    }
}
