using System;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Async
{
    internal sealed class CancelTaskCommand : ICommand
    {
        private readonly ITaskExecutor _executor;

        public CancelTaskCommand(ITaskExecutor executor)
        {
            _executor = executor;
            _executor.ProcessingStarted += OnProcessingChanged;
            _executor.ProcessingFinished += OnProcessingChanged;
        }

        private void OnProcessingChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _executor.IsRunning;
        }

        public void Execute(object parameter)
        {
            _executor.Cancel();
        }
    }
}
