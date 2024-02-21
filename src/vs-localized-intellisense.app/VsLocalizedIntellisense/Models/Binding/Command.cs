using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VsLocalizedIntellisense.Models.Binding
{
    /// <summary>
    /// <see cref="ICommand"/>実装基底クラス。
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        #region property

        /// <summary>
        /// 非同期処理か。
        /// </summary>
        public abstract bool IsAsyncMode { get; }

        /// <summary>
        /// 実行中か。
        /// </summary>
        public bool Executing { get; protected set; }

        #endregion

        #region ICommand

#pragma warning disable CS0067 // イベント 'CommandBase.CanExecuteChanged' は使用されていません
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // イベント 'CommandBase.CanExecuteChanged' は使用されていません

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

        private Action<TParameter> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override bool IsAsyncMode => false;

        public override void Execute(object parameter)
        {
            Executing = true;
            try
            {
                ExecuteAction((TParameter)parameter);
            }
            finally
            {
                Executing = false;
            }
        }

        public override bool CanExecute(object parameter)
        {
            return !Executing && CanExecuteFunc((TParameter)parameter);
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
        public AsyncDelegateCommandBase(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc ?? throw new ArgumentNullException(nameof(canExecuteFunc));
        }

        public AsyncDelegateCommandBase(Func<TParameter, Task> executeAction)
            : this(executeAction, EmptyCanExecuteFunc)
        { }

        #region property

        private Func<TParameter, Task> ExecuteAction { get; }
        private Func<TParameter, bool> CanExecuteFunc { get; }

        #endregion

        #region function

        private static bool EmptyCanExecuteFunc(TParameter parameter) => true;

        #endregion

        #region CommandBase

        public override bool IsAsyncMode => true;

        public override async void Execute(object parameter)
        {
            Executing = true;
            try
            {
                await ExecuteAction((TParameter)parameter);
            }
            finally
            {
                Executing = false;
            }
        }

        public override bool CanExecute(object parameter)
        {
            return !Executing && CanExecuteFunc((TParameter)parameter);
        }

        #endregion
    }

    public class AsyncDelegateCommand : AsyncDelegateCommandBase<object>
    {
        public AsyncDelegateCommand(Func<object, Task> executeAction) : base(executeAction)
        {
        }

        public AsyncDelegateCommand(Func<object, Task> executeAction, Func<object, bool> canExecuteFunc) : base(executeAction, canExecuteFunc)
        {
        }
    }

    public class AsyncDelegateCommand<TParameter> : AsyncDelegateCommandBase<TParameter>
    {
        public AsyncDelegateCommand(Func<TParameter, Task> executeAction) : base(executeAction)
        {
        }

        public AsyncDelegateCommand(Func<TParameter, Task> executeAction, Func<TParameter, bool> canExecuteFunc) : base(executeAction, canExecuteFunc)
        {
        }
    }
}
