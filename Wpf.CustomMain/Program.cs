using System;

namespace Wpf.CustomMain
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var app = new App();
                app.InitializeComponent();
                var mainWindow = new MainWindow();
                app.Run(mainWindow);
            }
            catch
            {
                //log
            }
        }
    }
}
