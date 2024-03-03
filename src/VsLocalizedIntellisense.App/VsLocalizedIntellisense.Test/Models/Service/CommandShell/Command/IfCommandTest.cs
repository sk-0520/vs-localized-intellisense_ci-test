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
    public class IfErrorLevelCommandTest
    {
        [TestMethod]
        public void IfTest()
        {
            var test = new IfErrorLevelCommand();
            test.Level = 10;
            test.TrueBlock.Add(new EchoCommand() { Value = "true" });

            var actual = test.GetStatement();
            Assert.AreEqual(
                "if ERRORLEVEL 10 (" + Environment.NewLine +
                "\techo true" + Environment.NewLine +
                ")" + Environment.NewLine,
                actual
            );
        }

        [TestMethod]
        public void IfElseTest()
        {
            var test = new IfErrorLevelCommand();
            test.Level = 10;
            test.TrueBlock.Add(new EchoCommand() { Value = "true" });
            test.FalseBlock.Add(new EchoCommand() { Value = "false" });

            var actual = test.GetStatement();
            Assert.AreEqual(
                "if ERRORLEVEL 10 (" + Environment.NewLine +
                "\techo true" + Environment.NewLine +
                ") else (" + Environment.NewLine +
                "\techo false" + Environment.NewLine +
                ")" + Environment.NewLine,
                actual
            );
        }
    }
}
