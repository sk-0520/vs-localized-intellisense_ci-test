using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models.Service.CommandShell;

namespace VsLocalizedIntellisense.Test.Models.Service.CommandShell
{
    [TestClass]
    public class RedirectBaseTest
    {
        private class TestRedirect : RedirectBase
        { }

        #region function

        [TestMethod]
        [DataRow("> a", "a", false)]
        [DataRow(">> a", "a", true)]
        public void ExpressionTest(string expected, string target, bool append)
        {
            var test = new TestRedirect()
            {
                Target = target,
                Append = append,
            };
            var actual = test.Expression;
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }

    [TestClass]
    public class RedirectTest
    {
        #region function

        [TestMethod]
        public void Expression_none_Test()
        {
            var test = new Redirect();
            Assert.AreEqual("", test.Expression);
        }

        [TestMethod]
        public void Expression_std_Test()
        {
            var test = new Redirect()
            {
                Target = "OUT",
            };
            Assert.AreEqual("> OUT", test.Expression);
        }

        [TestMethod]
        public void Expression_error_StandardOutput_Test()
        {
            var test = new Redirect()
            {
                Error = new ErrorRedirect()
                {
                    StandardOutput = true,
                },
            };
            Assert.AreEqual("2>&1", test.Expression);

            test.Target = "OUT";
            Assert.AreEqual("> OUT 2>&1", test.Expression);
        }

        [TestMethod]
        public void Expression_error_redirect_Test()
        {
            var test = new Redirect()
            {
                Error = new ErrorRedirect()
                {
                    Target = "ERR"
                },
            };
            Assert.AreEqual("2> ERR", test.Expression);

            test.Target = "OUT";
            Assert.AreEqual("> OUT 2> ERR", test.Expression);
        }

        [TestMethod]
        public void Expression_error_none_Test()
        {
            var test = new Redirect()
            {
                Error = new ErrorRedirect(),
            };
            Assert.AreEqual("", test.Expression);
        }

        #endregion
    }
}
