using System;
using System.Windows;

namespace Wpf.Async
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel?.Dispose();

            base.OnClosed(e);
        }
    }
}
