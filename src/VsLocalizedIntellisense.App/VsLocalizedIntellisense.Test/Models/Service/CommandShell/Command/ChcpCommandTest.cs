using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models.Service.CommandShell.Command;

namespace VsLocalizedIntellisense.Test.Models.Service.CommandPrompt.Command
{
    [TestClass]
    public class ChcpCommandTest
    {
        [TestMethod]
        public void Utf8_Test()
        {
            var chcp = new ChcpCommand
            {
                Encoding = Encoding.UTF8
            };
            var actual = chcp.GetStatement();
            Assert.AreEqual("chcp 65001", actual);
        }

        [TestMethod]
        public void ShiftJis_Test()
        {
            var chcp = new ChcpCommand()
            {
                Encoding = Encoding.GetEncoding("Shift_JIS"),
            };
            var actual = chcp.GetStatement();
            Assert.AreEqual("chcp 932", actual);
        }

        [TestMethod]
        public void Default_Test()
        {
            var chcp = new ChcpCommand();
            var actual = chcp.GetStatement();
            Assert.AreEqual($"chcp {Encoding.Default.CodePage}", actual);
        }

    }
}
