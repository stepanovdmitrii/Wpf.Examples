using System;
using System.Windows;
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
            if (Application.Current.Dispatcher.CheckAccess())
            {
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => { CommandManager.InvalidateRequerySuggested(); });
            }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
