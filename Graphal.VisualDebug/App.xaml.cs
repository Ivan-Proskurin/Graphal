using System.Windows;
using Graphal.VisualDebug.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Graphal.VisualDebug
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainViewModel = new ServiceCollection()
                .BuildVisualDebugContainer()
                .BuildServiceProvider()
                .GetRequiredService<IMainWindowViewModel>();
            
            await mainViewModel.InitializeAsync();
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel,
            };
            MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}