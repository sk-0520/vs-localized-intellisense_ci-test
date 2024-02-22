using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VsLocalizedIntellisense.Models
{
    /// <summary>
    /// <see cref="IDisposable.Dispose"/>が行われたかどうかを確認できるようにする。
    /// </summary>
    public interface IDisposed : IDisposable
    {
        #region property

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        bool IsDisposed { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="IDisposable.Dispose"/>をサポートする基底クラス。
    /// </summary>
    public abstract class DisposerBase : IDisposed
    {
        ~DisposerBase()
        {
            Dispose(false);
        }

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// 既に破棄済みの場合は処理を中断する。
        /// </summary>
        /// <param name="_callerMemberName"></param>
        /// <exception cref="ObjectDisposedException">破棄済み。</exception>
        /// <seealso cref="IDisposed"/>
        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }


        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    /// <summary>
    /// その場で破棄する処理。
    /// <para><c>using var xxx = new ActionDisposer(d => ...)</c>で実装する前提。</para>
    /// </summary>
    public sealed class ActionDisposer : DisposerBase
    {
        public ActionDisposer(Action<bool> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        #region property

        private Action<bool> Action { get; set; }

        #endregion

        #region ActionDisposer

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (Action != null)
                {
                    Action(disposing);
                    Action = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <inheritdoc cref="ActionDisposer"/>
    public sealed class ActionDisposer<TArgument> : DisposerBase
    {
        public ActionDisposer(Action<bool, TArgument> action, TArgument argument)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Argument = argument;
        }

        #region property

        private Action<bool, TArgument> Action { get; set; }
        private TArgument Argument { get; set; }

        #endregion

        #region ActionDisposer

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (Action != null)
                {
                    Action(disposing, Argument);
                    Action = null;
                    Argument = default;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ActionDisposer"/>, <see cref="ActionDisposer{TArgument}"/> の生成ヘルパー。
    /// </summary>
    public static class ActionDisposerHelper
    {
        #region define

        private sealed class EmptyDisposer : IDisposable
        {
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region function

        public static ActionDisposer Create(Action<bool> action) => new ActionDisposer(action);
        public static ActionDisposer<TArgument> Create<TArgument>(Action<bool, TArgument> action, TArgument argument) => new ActionDisposer<TArgument>(action, argument);

        /// <summary>
        /// <see cref="IDisposable"/>とのIFを合わせるための空処理。
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateEmpty() => new EmptyDisposer();

        #endregion
    }
}
