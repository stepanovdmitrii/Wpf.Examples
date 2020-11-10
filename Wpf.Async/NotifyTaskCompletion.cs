using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Wpf.Async
{
    //https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/march/async-programming-patterns-for-asynchronous-mvvm-applications-data-binding
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        private readonly Task<TResult> _task;
        public NotifyTaskCompletion(Task<TResult> task)
        {
            _task = task;
            if (!_task.IsCompleted)
            {
                var _ = WatchTaskAsync();
            }
        }
        private async Task WatchTaskAsync()
        {
            try
            {
                await _task;
            }
            catch
            {
            }
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            if (_task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            }
            else if (_task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
            }
            else
            {
                propertyChanged(this,
                  new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
            }
        }
        public TResult Result => _task.Status == TaskStatus.RanToCompletion ? _task.Result : default(TResult);
        public TaskStatus Status => _task.Status;
        public bool IsCompleted => _task.IsCompleted;
        public bool IsSuccessfullyCompleted => _task.Status == TaskStatus.RanToCompletion;
        public bool IsCanceled => _task.IsCanceled;
        public bool IsFaulted => _task.IsFaulted;
        public string ErrorMessage => _task.Exception?.InnerException?.Message ?? _task.Exception?.Message;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
