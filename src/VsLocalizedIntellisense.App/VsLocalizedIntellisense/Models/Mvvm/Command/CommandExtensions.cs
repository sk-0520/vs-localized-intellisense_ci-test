namespace VsLocalizedIntellisense.Models.Mvvm.Command
{
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
