using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models.Service.CommandPrompt;
using VsLocalizedIntellisense.Models.Service.CommandPrompt.Command;

namespace VsLocalizedIntellisense.Test.Models.Service.CommandPrompt.Command
{
    [TestClass]
    public class EchoTest
    {
        #region function

        [TestMethod]
        [DataRow("echo")]
        //[DataRow("echo a", "a")]
        public void Test(string expected, params string[] arguments)
        {
            var command = arguments.Length == 0
                ? new Echo()
                : new Echo()
                {
                    Value = new CommandValue(arguments),
                }
            ;
            var actual = command.GetStatement();
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
