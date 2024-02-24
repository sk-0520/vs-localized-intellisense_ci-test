using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Collections.Specialized.BitVector32;

namespace VsLocalizedIntellisense.Models.Mvvm
{
    /// <summary>
    /// メッセージ内容。
    /// <para>登録時の型とIDがキーとなる。</para>
    /// <para>入力値・コールバックなどを必要に応じて実装していく。</para>
    /// </summary>
    public interface IMessage
    {
        #region property

        /// <summary>
        /// メッセージを特定するID。
        /// <para>メッセージは最初に見つかったものが使用されるため同じメッセージに対して処理を振り分けるために使用する。</para>
        /// </summary>
        string MessageId { get; }

        #endregion
    }

    /// <summary>
    /// 保持しているメッセージ。
    /// </summary>
    public class MessageItem : DisposerBase
    {
        public MessageItem(string messageId, Type messageType, object callbackInstance, MethodInfo callbackMethodInfo)
        {
            MessageId = messageId;
            MessageType = messageType;
            CallbackInstance = callbackInstance;
            CallbackMethodInfo = callbackMethodInfo;
        }

        #region property

        /// <inheritdoc cref="IMessage.MessageId"/>
        public string MessageId { get; }
        /// <summary>
        /// <see cref="IMessage"/> の型情報。
        /// </summary>
        public Type MessageType { get; }
        /// <summary>
        /// 登録された処理のインスタンス。
        /// </summary>
        public object CallbackInstance { get; private set; }
        /// <summary>
        /// 登録された処理のメソッド情報。
        /// </summary>
        public MethodInfo CallbackMethodInfo { get; private set; }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                CallbackInstance = null;
                CallbackMethodInfo = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    public interface IReceivableMessenger
    {
        MessageItem Register<TMessage>(Action<TMessage> action, string messageId = "") where TMessage : IMessage;
        MessageItem Register<TMessage>(Func<TMessage, Task> callback, string messageId = "") where TMessage : IMessage;
        void Unregister(MessageItem messageItem);
    }

    public interface ISendableMessenger
    {
        void Send<TMessage>(TMessage message) where TMessage : IMessage;
        Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : IMessage;
    }

    public interface IMessenger : IReceivableMessenger, ISendableMessenger
    { }


    public class Messenger : DisposerBase, IMessenger
    {
        public Messenger()
        { }

        #region property

        private Dictionary<Type, List<MessageItem>> RegisteredSyncTypes { get; } = new Dictionary<Type, List<MessageItem>>();
        private Dictionary<Type, List<MessageItem>> RegisteredAsyncTypes { get; } = new Dictionary<Type, List<MessageItem>>();

        #endregion

        #region function

        public MessageItem Register<TMessage>(Action<TMessage> action, string messageId = "")
            where TMessage : IMessage
        {
            var messageType = typeof(TMessage);

            if (!RegisteredSyncTypes.TryGetValue(messageType, out var messages))
            {
                messages = new List<MessageItem>();
                RegisteredSyncTypes.Add(messageType, messages);
            }

            var item = new MessageItem(messageId, messageType, action.Target, action.Method);
            messages.Add(item);
            return item;
        }

        public MessageItem Register<TMessage>(Func<TMessage, Task> callback, string messageId = "")
            where TMessage : IMessage
        {
            var messageType = typeof(TMessage);

            if (!RegisteredAsyncTypes.TryGetValue(messageType, out var messages))
            {
                messages = new List<MessageItem>();
                RegisteredAsyncTypes.Add(messageType, messages);
            }

            var item = new MessageItem(messageId, messageType, callback.Target, callback.Method);
            messages.Add(item);
            return item;
        }

        private bool UnregisterCore(MessageItem messageItem, Dictionary<Type, List<MessageItem>> registeredTypes)
        {
            if (registeredTypes.TryGetValue(messageItem.MessageType, out var messages))
            {
                if (messages.Remove(messageItem))
                {
                    messageItem.Dispose();

                    return true;
                }
            }

            return false;
        }

        public void Unregister(MessageItem messageItem)
        {
            ThrowIfDisposed();

            if (!UnregisterCore(messageItem, RegisteredSyncTypes))
            {
                UnregisterCore(messageItem, RegisteredAsyncTypes);
            }
        }

        private MessageItem SearchMessageItem(IMessage needle, IReadOnlyList<MessageItem> haystack)
        {
            return haystack.FirstOrDefault(a => a.MessageId == needle.MessageId);
        }

        public void Send<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            if (!RegisteredSyncTypes.TryGetValue(typeof(TMessage), out var messages))
            {
                return;
            }

            var messageItem = SearchMessageItem(message, messages);
            if (messageItem == null)
            {
                return;
            }

            messageItem.CallbackMethodInfo.Invoke(messageItem.CallbackInstance, new object[] { message });
        }

        public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            if (!RegisteredAsyncTypes.TryGetValue(typeof(TMessage), out var messages))
            {
                return Task.CompletedTask;
            }

            var messageItem = SearchMessageItem(message, messages);
            if (messageItem == null)
            {
                return Task.CompletedTask;
            }

            var result = messageItem.CallbackMethodInfo.Invoke(messageItem.CallbackInstance, new object[] { message });

            return (Task)result;
        }

        #endregion

        #region DisposerBase

        private static void DisposeTypes(Dictionary<Type, List<MessageItem>> registeredTypes)
        {
            foreach (var pair in registeredTypes)
            {
                foreach (var item in pair.Value)
                {
                    item.Dispose();
                }
                pair.Value.Clear();
            }
            registeredTypes.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                DisposeTypes(RegisteredSyncTypes);
                DisposeTypes(RegisteredAsyncTypes);
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class ScopedMessenger : DisposerBase, IMessenger
    {
        public ScopedMessenger(IMessenger messenger)
        {
            Messenger = messenger;
        }

        #region property

        private IMessenger Messenger { get; }
        private ISet<MessageItem> ScopeItems { get; } = new HashSet<MessageItem>();

        #endregion

        #region IMessenger

        public MessageItem Register<TMessage>(Action<TMessage> action, string messageId = "")
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            var messageItem = Messenger.Register(action, messageId);
            ScopeItems.Add(messageItem);

            return messageItem;
        }

        public MessageItem Register<TMessage>(Func<TMessage, Task> callback, string messageId = "")
            where TMessage : IMessage
        {
            ThrowIfDisposed();

            var messageItem = Messenger.Register(callback, messageId);
            ScopeItems.Add(messageItem);

            return messageItem;
        }

        public void Unregister(MessageItem messageItem)
        {
            ThrowIfDisposed();

            if (ScopeItems.Contains(messageItem))
            {
                Messenger.Unregister(messageItem);
                ScopeItems.Remove(messageItem);
            }
        }

        public void Send<TMessage>(TMessage message)
            where TMessage : IMessage
            => Messenger.Send(message)
        ;

        public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage
            => Messenger.SendAsync(message, cancellationToken)
        ;

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                var items = ScopeItems.ToArray();
                foreach (var item in items)
                {
                    Unregister(item);
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }


    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MessengerAttribute : Attribute
    {
        // This is a positional argument
        public MessengerAttribute()
        { }
    }

    public static class MessengerHelper
    {
        #region function

        /// <summary>
        /// <see cref="MessengerAttribute"/>で指定された<see cref="IMessenger"/>型のプロパティを抜き出す。
        /// <para>TODO: 本処理は直接使用せずさらにヘルパーを噛ませる想定。</para>
        /// </summary>
        /// <param name="dataContext">対象オブジェクト。</param>
        /// <param name="rawMessenger">プロパティの値を直接取得するか。<see langword="true" />の場合直接取得し、<see langword="false" />(もしくは未指定)の場合は<see cref="ScopedMessenger"/>として取得する。</param>
        /// <returns>取得した<see cref="IMessenger"/>。取得できなかった場合は<see langword="null"/>を返す。</returns>
        public static IMessenger GetMessengerFromProperty(object dataContext, bool rawMessenger = false)
        {
            var type = dataContext.GetType();
            var properties = type.GetProperties();
            var messengerAttribute = properties
                .Select(a => (property: a, messenger: a.GetCustomAttribute<MessengerAttribute>()))
                .FirstOrDefault(a => a.messenger != null)
            ;

            if (messengerAttribute.messenger == null)
            {
                return null;
            }

            var value = messengerAttribute.property.GetValue(dataContext);
            if (value is IMessenger messenger)
            {
                if (rawMessenger)
                {
                    return messenger;
                }

                return new ScopedMessenger(messenger);
            }

            return null;
        }

        #endregion
    }

    public class ViewMessenger<TView> : DisposerBase
        where TView : FrameworkElement
    {
        public ViewMessenger(TView element, Action<IReceivableMessenger> resisterAction)
        {
            Element = element;
            ResisterAction = resisterAction;

            element.Unloaded += Element_Unloaded;

            element.DataContextChanged += Element_DataContextChanged;

            if (element.IsLoaded)
            {
                Register();
            }
        }

        #region property

        private TView Element { get; set; }
        private ScopedMessenger Messenger { get; set; }
        private Action<IReceivableMessenger> ResisterAction { get; set; }

        #endregion

        #region function

        private void Register()
        {
            ThrowIfDisposed();

            Messenger = (ScopedMessenger)MessengerHelper.GetMessengerFromProperty(Element.DataContext);
            ResisterAction(Messenger);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (Element != null)
                {
                    Element.DataContextChanged -= Element_DataContextChanged;
                }
                Element = null;

                if (disposing)
                {
                    Messenger.Dispose();
                }
                Messenger = null;
                ResisterAction = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Element_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Messenger?.Dispose();

            if (e.NewValue != null)
            {
                Debug.Assert(e.NewValue == Element.DataContext);
                Register();
            }
        }

        private void Element_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }
    }
}
