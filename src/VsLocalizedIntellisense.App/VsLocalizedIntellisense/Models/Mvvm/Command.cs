using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows.Input;

namespace VsLocalizedIntellisense.Models.Mvvm
{
    /// <summary>
    /// <see cref="ICommand"/>実装基底クラス。
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        protected CommandBase()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }

        #region property

        protected SynchronizationContext SynchronizationContext { get; }

        /// <summary>
        /// 同時実行を抑制するか。
        /// <para>基本的に <see langword="init"/> であることを前提としてる。使えんけど。</para>
        /// </summary>
        public bool SuppressCommandWhileExecuting { get; set; } = true;

        #endregion

        #region function

        protected virtual void OnCanExecuteChanged()
        {
            if (SynchronizationContext != SynchronizationContext.Current)
            {
                SynchronizationContext.Post(o => CanExecuteChanged.Invoke(this, EventArgs.Empty), null);
            }
            else
            {
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        #endregion

        #region ICommand

#pragma warning disable CS0067 // イベント 'CommandBase.CanExecuteChanged' は使用されていません
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public abstract void Execute(object parameter);

        public abstract bool CanExecute(object parameter);

        #endregion
    }

    public abstract class DelegateCommandBase<TParameter> : CommandBase
    {
        public DelegateCommandBase(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        public DelegateCommandBase(Action<TParameter> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        /// <summary>
        /// 現在実行数。
        /// </summary>
        public int ExecutingCount { get; private set; }

        private Action<TParameter> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override void Execute(object parameter)
        {
            ExecutingCount += 1;
            try
            {
                ExecuteAction((TParameter)parameter);
            }
            finally
            {
                ExecutingCount -= 1;
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (SuppressCommandWhileExecuting)
            {
                return ExecutingCount == 0 && CanExecuteFunc((TParameter)parameter);
            }

            return CanExecuteFunc((TParameter)parameter);
        }

        #endregion
    }

    public class DelegateCommand : DelegateCommandBase<object>
    {
        public DelegateCommand(Action<object> executeAction)
            : base(executeAction)
        { }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }

    public class DelegateCommand<TParameter> : DelegateCommandBase<TParameter>
    {
        public DelegateCommand(Action<TParameter> executeAction)
            : base(executeAction)
        { }

        public DelegateCommand(Action<TParameter> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }


    public abstract class AsyncDelegateCommandBase<TParameter> : CommandBase
    {
        #region variable

        private int _executingCount;

        #endregion

        public AsyncDelegateCommandBase(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        public AsyncDelegateCommandBase(Func<TParameter, Task> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        /// <summary>
        /// 現在実行数。
        /// </summary>
        public int ExecutingCount => this._executingCount;

        private Func<TParameter, Task> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override async void Execute(object parameter)
        {
            Interlocked.Increment(ref this._executingCount);
            try
            {
                await ExecuteAction((TParameter)parameter);
            }
            finally
            {
                Interlocked.Decrement(ref this._executingCount);
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (SuppressCommandWhileExecuting)
            {
                return ExecutingCount == 0 && CanExecuteFunc((TParameter)parameter);
            }

            return CanExecuteFunc((TParameter)parameter);
        }

        #endregion
    }

    public class AsyncDelegateCommand : AsyncDelegateCommandBase<object>
    {
        public AsyncDelegateCommand(Func<object, Task> executeAction)
            : base(executeAction)
        { }

        public AsyncDelegateCommand(Func<object, Task> executeAction, Func<object, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }

    public class AsyncDelegateCommand<TParameter> : AsyncDelegateCommandBase<TParameter>
    {
        public AsyncDelegateCommand(Func<TParameter, Task> executeAction)
            : base(executeAction)
        { }

        public AsyncDelegateCommand(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
            : base(executeAction, canExecuteFunc)
        { }
    }

    public static class CommandExtensions
    {
        #region function

        public static void Invoke<TParameter>(this DelegateCommandBase<TParameter> command, TParameter parameter)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        public static void Invoke<TParameter>(this AsyncDelegateCommandBase<TParameter> command, TParameter parameter)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        #endregion
    }
}
