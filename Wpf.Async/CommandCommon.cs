using System;
using System.Windows.Input;

namespace Wpf.Async
{
    internal abstract class CommandCommon : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
