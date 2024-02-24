using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsLocalizedIntellisense.Models.Mvvm;

namespace VsLocalizedIntellisense.Test.Models.Mvvm
{
    [TestClass]
    public class MessengerTest
    {
        #region function

        private class ActionMessage : IMessage
        {
            public ActionMessage(string messageId = "")
            {
                MessageId = messageId;
            }

            public string MessageId { get; }
        }

        [TestMethod]
        public void Scenario_Action_Test()
        {
            var messenger = new Messenger();
            var callCount = 0;
            var messageItem = messenger.Register<ActionMessage>(m => callCount += 1);

            Assert.AreEqual(0, callCount);
            messenger.Send(new ActionMessage());
            Assert.AreEqual(1, callCount);

            messenger.Send(new ActionMessage("no-hit"));
            Assert.AreEqual(1, callCount);

            messenger.Send(new ActionMessage());
            Assert.AreEqual(2, callCount);

            messenger.Unregister(messageItem);

            messenger.Send(new ActionMessage());
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task Scenario_Function_Test()
        {
            var messenger = new Messenger();
            var callCount = 0;
            var messageItem = messenger.Register<ActionMessage>(m => Task.FromResult(callCount += 1));

            Assert.AreEqual(0, callCount);
            messenger.Send(new ActionMessage());
            Assert.AreEqual(0, callCount);

            await messenger.SendAsync(new ActionMessage());
            Assert.AreEqual(1, callCount);

            await messenger.SendAsync(new ActionMessage("no-hit"));
            Assert.AreEqual(1, callCount);

            await messenger.SendAsync(new ActionMessage());
            Assert.AreEqual(2, callCount);

            messenger.Unregister(messageItem);

            await messenger.SendAsync(new ActionMessage());
            Assert.AreEqual(2, callCount);

        }

        #endregion
    }

    [TestClass]
    public class ScopedMessengerTest
    {
        #region function

        private class ActionMessage : IMessage
        {
            public ActionMessage(string messageId = "")
            {
                MessageId = messageId;
            }

            public string MessageId { get; }
        }

        private class ActionMessage2 : ActionMessage
        {
            public ActionMessage2(string messageId = "")
                : base(messageId)
            { }
        }

        [TestMethod]
        public void Scenario_normal_Test()
        {
            var rootMessenger = new Messenger();
            var scopedMessenger = new ScopedMessenger(rootMessenger);

            var rootCallCount = 0;
            var scopedCallCountNotCall = 0;
            var scopedCallCountCanCall = 0;

            var rootMessageItem = rootMessenger.Register<ActionMessage>(m => rootCallCount += 1);
            // 既に登録されてるので呼び出されない(スコープ云々関係なくID指定が必要なパターン)
            var scopedMessageItemNotCall = scopedMessenger.Register<ActionMessage>(m => scopedCallCountNotCall += 1);
            // 新たに登録した完全別物
            var scopedMessageItemCanCall = scopedMessenger.Register<ActionMessage2>(m => scopedCallCountCanCall += 1);

            rootMessenger.Send(new ActionMessage());
            Assert.AreEqual(1, rootCallCount);
            Assert.AreEqual(0, scopedCallCountNotCall);
            Assert.AreEqual(0, scopedCallCountCanCall);

            // rootMessenger に登録済みのものが呼び出される(既に登録されてるのでIDが必要パターン)
            scopedMessenger.Send(new ActionMessage());
            Assert.AreEqual(2, rootCallCount);
            Assert.AreEqual(0, scopedCallCountNotCall);
            Assert.AreEqual(0, scopedCallCountCanCall);

            // スコープ側での登録処理は最終的に root に伝わる
            rootMessenger.Send(new ActionMessage2());
            Assert.AreEqual(2, rootCallCount);
            Assert.AreEqual(0, scopedCallCountNotCall);
            Assert.AreEqual(1, scopedCallCountCanCall);

            // これはまぁふつう
            scopedMessenger.Send(new ActionMessage2());
            Assert.AreEqual(2, rootCallCount);
            Assert.AreEqual(0, scopedCallCountNotCall);
            Assert.AreEqual(2, scopedCallCountCanCall);

            // 破棄したら root でも呼び出し無効
            scopedMessenger.Dispose();
            Assert.IsTrue(scopedMessageItemNotCall.IsDisposed);
            Assert.IsTrue(scopedMessageItemCanCall.IsDisposed);

            rootMessenger.Send(new ActionMessage2());
            Assert.AreEqual(2, rootCallCount);
            Assert.AreEqual(0, scopedCallCountNotCall);
            Assert.AreEqual(2, scopedCallCountCanCall);
        }

        #endregion  
    }

    [TestClass]
    public class MessengerHelperTest
    {
        #region function

        private class TestGetMessengerFromProperty_empty
        { }

        [TestMethod]
        public void GetMessengerFromProperty_empty_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_empty();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.IsNull(actual);
        }


        private class TestGetMessengerFromProperty_private
        {
            [Messenger]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private Messenger Messenger { get; } = new Messenger();
        }

        [TestMethod]
        public void GetMessengerFromProperty_private_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_private();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.IsNull(actual);
        }

        private class TestGetMessengerFromProperty_type1
        {
            [Messenger]
            public Uri Messenger { get; } = new Uri("file://NUL");
        }

        [TestMethod]
        public void GetMessengerFromProperty_type1_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_type1();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.IsNull(actual);
        }

        private class TestGetMessengerFromProperty_type2
        {
            [Messenger]
            public object Messenger { get; } = new object();
        }

        [TestMethod]
        public void GetMessengerFromProperty_type2_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_type2();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.IsNull(actual);
        }

        private class TestGetMessengerFromProperty_null
        {
            [Messenger]
            public Messenger Messenger { get; } = null;
        }

        [TestMethod]
        public void GetMessengerFromProperty_null_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_null();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.IsNull(actual);
        }

        private class TestGetMessengerFromProperty_success
        {
            [Messenger]
            public Messenger Messenger { get; } = new Messenger();
        }

        [TestMethod]
        public void TestGetMessengerFromProperty_success_scoped_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_success();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(ScopedMessenger));
        }

        [TestMethod]
        public void TestGetMessengerFromProperty_success_raw_Test()
        {
            var dataContext = new TestGetMessengerFromProperty_success();
            var actual = MessengerHelper.GetMessengerFromProperty(dataContext, true);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(Messenger));
        }

        #endregion
    }
}
