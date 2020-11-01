using System;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Async
{

    internal sealed class StartTaskCommand: ICommand
    {
        private readonly ITaskExecutor _taskExecutor;

        public event EventHandler CanExecuteChanged;

        public StartTaskCommand(ITaskExecutor taskExecutor)
        {
            _taskExecutor = taskExecutor;
            _taskExecutor.ProcessingStarted += OnProcessingChanged;
            _taskExecutor.ProcessingFinished += OnProcessingChanged;
        }

        private void OnProcessingChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        public bool CanExecute(object parameter)
        {
            return !_taskExecutor.IsRunning;
        }

        public void Execute(object parameter)
        {
            _taskExecutor.Start();
        }
    }
}
