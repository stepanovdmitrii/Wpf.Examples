using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf.Async
{
    //https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands
    internal interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
