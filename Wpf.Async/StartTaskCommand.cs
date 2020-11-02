namespace Wpf.Async
{

    internal sealed class StartTaskCommand: CommandCommon
    {
        private readonly ITaskExecutor _taskExecutor;

        public StartTaskCommand(ITaskExecutor taskExecutor)
        {
            _taskExecutor = taskExecutor;
            _taskExecutor.ProcessingStarted += OnProcessingChanged;
            _taskExecutor.ProcessingFinished += OnProcessingChanged;
        }

        private void OnProcessingChanged()
        {
            RaiseCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return !_taskExecutor.IsRunning;
        }

        public override void Execute(object parameter)
        {
            _taskExecutor.Start();
        }
    }
}
