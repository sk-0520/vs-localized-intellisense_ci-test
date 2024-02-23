using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.Test.Models.Logger
{
    [TestClass]
    public class LoggingTest
    {
        #region function

        [TestMethod]
        // Trace
        [DataRow(true, LogLevel.Trace, LogLevel.Trace)]
        [DataRow(true, LogLevel.Trace, LogLevel.Debug)]
        [DataRow(true, LogLevel.Trace, LogLevel.Information)]
        [DataRow(true, LogLevel.Trace, LogLevel.Warning)]
        [DataRow(true, LogLevel.Trace, LogLevel.Error)]
        [DataRow(true, LogLevel.Trace, LogLevel.Critical)]
        [DataRow(false, LogLevel.Trace, LogLevel.None)]
        // Debug
        [DataRow(false, LogLevel.Debug, LogLevel.Trace)]
        [DataRow(true, LogLevel.Debug, LogLevel.Debug)]
        [DataRow(true, LogLevel.Debug, LogLevel.Information)]
        [DataRow(true, LogLevel.Debug, LogLevel.Warning)]
        [DataRow(true, LogLevel.Debug, LogLevel.Error)]
        [DataRow(true, LogLevel.Debug, LogLevel.Critical)]
        [DataRow(false, LogLevel.Debug, LogLevel.None)]
        // Information
        [DataRow(false, LogLevel.Information, LogLevel.Trace)]
        [DataRow(false, LogLevel.Information, LogLevel.Debug)]
        [DataRow(true, LogLevel.Information, LogLevel.Information)]
        [DataRow(true, LogLevel.Information, LogLevel.Warning)]
        [DataRow(true, LogLevel.Information, LogLevel.Error)]
        [DataRow(true, LogLevel.Information, LogLevel.Critical)]
        [DataRow(false, LogLevel.Information, LogLevel.None)]
        // Warning
        [DataRow(false, LogLevel.Warning, LogLevel.Trace)]
        [DataRow(false, LogLevel.Warning, LogLevel.Debug)]
        [DataRow(false, LogLevel.Warning, LogLevel.Information)]
        [DataRow(true, LogLevel.Warning, LogLevel.Warning)]
        [DataRow(true, LogLevel.Warning, LogLevel.Error)]
        [DataRow(true, LogLevel.Warning, LogLevel.Critical)]
        [DataRow(false, LogLevel.Warning, LogLevel.None)]
        // Error
        [DataRow(false, LogLevel.Error, LogLevel.Trace)]
        [DataRow(false, LogLevel.Error, LogLevel.Debug)]
        [DataRow(false, LogLevel.Error, LogLevel.Information)]
        [DataRow(false, LogLevel.Error, LogLevel.Warning)]
        [DataRow(true, LogLevel.Error, LogLevel.Error)]
        [DataRow(true, LogLevel.Error, LogLevel.Critical)]
        [DataRow(false, LogLevel.Error, LogLevel.None)]
        // Critical
        [DataRow(false, LogLevel.Critical, LogLevel.Trace)]
        [DataRow(false, LogLevel.Critical, LogLevel.Debug)]
        [DataRow(false, LogLevel.Critical, LogLevel.Information)]
        [DataRow(false, LogLevel.Critical, LogLevel.Warning)]
        [DataRow(false, LogLevel.Critical, LogLevel.Error)]
        [DataRow(true, LogLevel.Critical, LogLevel.Critical)]
        [DataRow(false, LogLevel.Critical, LogLevel.None)]
        // None
        [DataRow(false, LogLevel.None, LogLevel.Trace)]
        [DataRow(false, LogLevel.None, LogLevel.Debug)]
        [DataRow(false, LogLevel.None, LogLevel.Information)]
        [DataRow(false, LogLevel.None, LogLevel.Warning)]
        [DataRow(false, LogLevel.None, LogLevel.Error)]
        [DataRow(false, LogLevel.None, LogLevel.Critical)]
        [DataRow(false, LogLevel.None, LogLevel.None)]
        public void IsEnabledTest(bool expected, LogLevel defaultLevel, LogLevel compareLevel)
        {
            var actual = Logging.IsEnabled(defaultLevel, compareLevel);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
