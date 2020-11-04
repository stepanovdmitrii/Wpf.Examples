using System;
using System.Threading;

namespace Wpf.Async
{
    internal interface ITaskExecutor
    {
        event Action ProcessingStarted;
        event Action ProcessingFinished;
        void Start();
        void Cancel();
        bool IsRunning { get; }
    }

    internal sealed class TaskExecutor : ITaskExecutor, IDisposable
    {
        private readonly object _guard = new object();
        private readonly IProgress<int> _progress;

        private CancellationTokenSource _tokenSource;
        private Thread _thread;
        private bool _isRunning;

        public bool IsRunning
        {
            get
            {
                lock (_guard)
                {
                    return _isRunning;
                }
            }
        }

        public TaskExecutor(IProgress<int> progress)
        {
            _progress = progress;
            _isRunning = false;
        }


        public event Action ProcessingStarted;
        public event Action ProcessingFinished;

        public void Start()
        {
            lock (_guard)
            {
                if (_isRunning) return;

                _isRunning = true;
                using (_tokenSource)
                {
                    _tokenSource = null;
                }

                _tokenSource = new CancellationTokenSource();
                _thread = new Thread(Run);
                _thread.Start();
                _thread.Name = "Worker";
            }
        }

        private void Run()
        {
            try
            {
                ProcessingStarted?.Invoke();
                for (int i = 0; i < 100; ++i)
                {
                    _tokenSource.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(TimeSpan.FromSeconds(1.0));
                    _progress.Report(i);
                }
            }
            catch { }
            finally
            {
                _progress.Report(100);
                lock (_guard)
                {
                    _isRunning = false;
                }
                ProcessingFinished?.Invoke();
            }
        }


        public void Dispose()
        {
            _tokenSource?.Cancel();
            _thread?.Join();
            _tokenSource?.Dispose();
        }

        public void Cancel()
        {
            lock (_guard)
            {
                if (!_isRunning) return;
                _tokenSource?.Cancel();
            }
        }
    }
}
