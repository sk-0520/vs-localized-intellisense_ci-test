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
    public class TextTest
    {
        #region function

        [TestMethod]
        public void Test()
        {
            var test = new Text("abc");
            Assert.AreEqual("abc", test.Data);
            Assert.AreEqual("abc", test.Expression);
        }

        #endregion
    }

    [TestClass]
    public class VariableTest
    {
        #region function

        [TestMethod]
        public void NormalTest()
        {
            var test = new Variable("abc");
            Assert.AreEqual("abc", test.Name);
            Assert.AreEqual("%abc%", test.Expression);

            test.Name = "xyz";
            Assert.AreEqual("xyz", test.Name);
            Assert.AreEqual("%xyz%", test.Expression);
        }

        [TestMethod]
        public void DelayedExpansionTest()
        {
            var test = new Variable("abc")
            {
                DelayedExpansion = true,
            };
            Assert.AreEqual("abc", test.Name);
            Assert.AreEqual("!abc!", test.Expression);

            test.Name = "xyz";
            Assert.AreEqual("xyz", test.Name);
            Assert.AreEqual("!xyz!", test.Expression);
        }

        [TestMethod]
        public void IsReadOnlyTest()
        {
            var test = new Variable("abc", true);
            Assert.AreEqual("abc", test.Name);
            Assert.AreEqual("%abc%", test.Expression);

            Assert.ThrowsException<InvalidOperationException>(() => test.Name = "xyz");
            Assert.AreEqual("abc", test.Name);
            Assert.AreEqual("%abc%", test.Expression);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentException>(() => new Variable(""));
        }

        [TestMethod]
        public void Name_throw_Test()
        {
            var test = new Variable("abc");

            Assert.ThrowsException<ArgumentException>(() => test.Name = "");
        }

        #endregion
    }

    [TestClass]
    public class ValueTest
    {
        #region function

        [TestMethod]
        public void ImplicitOperatorTest()
        {
            Value test = "abc";
            Assert.AreEqual(1, test.Values.Count);
            Assert.IsInstanceOfType<Text>(test.Values[0]);
            Assert.AreEqual("abc", test.Expression);
        }

        [TestMethod]
        public void OperatorTest()
        {
            var text = new Text("abc");
            var variable = new Variable("xyz");
            var test1 = text + variable;
            var test2 = variable + text;
            var test3 = text + variable + text;
            Assert.AreEqual("abc%xyz%", test1.Expression);
            Assert.AreEqual("%xyz%abc", test2.Expression);
            Assert.AreEqual("abc%xyz%abc", test3.Expression);

            var test4 = test1 + test2 + test3;
            Assert.AreEqual("abc%xyz%%xyz%abcabc%xyz%abc", test4.Expression);
        }

        #endregion
    }
}
