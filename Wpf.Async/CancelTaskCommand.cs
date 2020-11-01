﻿using System;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Async
{
    internal sealed class CancelTaskCommand : CommandCommon
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
                RaiseCanExecuteChanged();
            });
        }

        public override bool CanExecute(object parameter)
        {
            return _executor.IsRunning;
        }

        public override void Execute(object parameter)
        {
            _executor.Cancel();
        }
    }
}
