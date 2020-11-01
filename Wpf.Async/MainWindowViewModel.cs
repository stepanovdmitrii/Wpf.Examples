using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Async
{
    internal sealed class MainWindowViewModel : INotifyPropertyChanged, IProgressListener, IDisposable
    {
        private readonly TaskExecutor _executor;
        private readonly StartTaskCommand _task;
        private readonly CancelTaskCommand _cancel;

        private int _progress;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            _executor = new TaskExecutor(this);
            _task = new StartTaskCommand(_executor);
            _cancel = new CancelTaskCommand(_executor);
            _progress = 0;
        }

        public ICommand StartCommand => _task;
        public ICommand CancelCommand => _cancel;

        public int Progress 
        {
            get => _progress;
            set
            {
                _progress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
            }
        }

        public void SetProgress(int progress)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (progress == 100)
                {
                    Progress = 0;
                }
                else
                {
                    Progress = progress;
                }
            });
        }

        public void Dispose()
        {
            _executor?.Dispose();
        }
    }
}
