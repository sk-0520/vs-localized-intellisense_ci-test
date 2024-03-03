using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models.Service.CommandShell.Command;

namespace VsLocalizedIntellisense.Test.Models.Service.CommandShell.Command
{
    [TestClass]
    public class CopyCommandTest
    {
        #region function

        [TestMethod]
        public void Source_throw_Test()
        {
            var test = new CopyCommand
            {
                Destination = "dst"
            };
            var e = Assert.ThrowsException<InvalidOperationException>(() => test.GetStatement());
            Assert.AreEqual(nameof(test.Source), e.Message);
        }

        [TestMethod]
        public void Destination_throw_Test()
        {
            var test = new CopyCommand
            {
                Source = "src",
            };
            var e = Assert.ThrowsException<InvalidOperationException>(() => test.GetStatement());
            Assert.AreEqual(nameof(test.Destination), e.Message);
        }

        [TestMethod]
        public void IsDecryptionTest()
        {
            var test = new CopyCommand
            {
                Source = "src",
                Destination = "dst",
            };
            Assert.AreEqual("copy src dst", test.GetStatement());

            test.IsDecryption = true;
            Assert.AreEqual("copy /d src dst", test.GetStatement());
        }

        [TestMethod]
        public void IsVerifyTest()
        {
            var test = new CopyCommand
            {
                Source = "src",
                Destination = "dst",
            };
            Assert.AreEqual("copy src dst", test.GetStatement());

            test.IsVerify = true;
            Assert.AreEqual("copy /v src dst", test.GetStatement());
        }

        [TestMethod]
        public void IsForceTest()
        {
            var test = new CopyCommand
            {
                Source = "src",
                Destination = "dst",
            };
            Assert.AreEqual("copy src dst", test.GetStatement());

            test.IsForce = false;
            Assert.AreEqual("copy /-y src dst", test.GetStatement());

            test.IsForce = true;
            Assert.AreEqual("copy /y src dst", test.GetStatement());

            test.IsForce = null;
            Assert.AreEqual("copy src dst", test.GetStatement());
        }

        #endregion
    }
}
