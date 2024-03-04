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
    public class CommandShellEditorTest
    {
        #region function

        #region add-command

        [TestMethod]
        public void AddChangeCodePageTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddChangeCodePage(Encoding.Default);

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual(Encoding.Default, actual.Encoding);
        }

        [TestMethod]
        public void AddChangeDirectoryTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddChangeDirectory("dir");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("dir", actual.Path.Expression);
            Assert.IsTrue(actual.WithDrive);
        }

        [TestMethod]
        public void AddChangeSelfDirectoryTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddChangeSelfDirectory();

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("cd /d %~dp0", actual.GetStatement());
        }

        [TestMethod]
        public void AddCopyTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddCopy("src", "dst");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("src", actual.Source.Expression);
            Assert.AreEqual("dst", actual.Destination.Expression);
            Assert.IsFalse(actual.IsForce);
        }

        [TestMethod]
        public void AddEchoTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddEcho("abc");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("abc", actual.Value.Expression);
        }

        [TestMethod]
        public void AddIfErrorLevelTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddIfErrorLevel(123);

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual(123, actual.Level);
            Assert.IsFalse(actual.IsNot);
        }

        [TestMethod]
        public void AddIfExpressTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddIfExpress("LEFT", "RIGHT");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("LEFT", actual.Left.Expression);
            Assert.AreEqual("RIGHT", actual.Right.Expression);
            Assert.IsFalse(actual.IsNot);
        }

        [TestMethod]
        public void AddIfExistTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddIfExist("path");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("path", actual.Path.Expression);
            Assert.IsFalse(actual.IsNot);
        }

        [TestMethod]
        public void AddRemarkTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddRemark("comment");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("comment", actual.Comment.Expression);
        }


        [TestMethod]
        public void AddSetVariableTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddSetVariable("var", "value");

            Assert.AreEqual(test.Actions[0], actual);
            Assert.AreEqual("var", actual.VariableName);
            Assert.IsFalse(actual.IsExpress);
            Assert.IsFalse(actual.Variable.IsReadOnly);
            Assert.IsFalse(actual.Variable.DelayedExpansion);
        }

        [TestMethod]
        public void AddSwitchEchoTest()
        {
            var test = new CommandShellEditor();
            var actual = test.AddSwitchEcho(true);

            Assert.AreEqual(test.Actions[0], actual);
            Assert.IsTrue(actual.On);
        }

        #endregion

        #endregion
    }
}
