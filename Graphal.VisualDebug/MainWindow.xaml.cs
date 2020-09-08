using System;
using System.Windows.Input;

using Graphal.VisualDebug.Abstractions;
using Graphal.VisualDebug.Helpers;

namespace Graphal.VisualDebug
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnClosed(object sender, EventArgs e)
        {
            if (DataContext is IMainWindowViewModel viewModel)
            {
                await viewModel.CloseAsync();
            }
        }

        private IMainWindowViewModel ViewModel => this.GetViewModel<IMainWindowViewModel>();

        private async void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (ViewModel == null) return;

            if (e.Key == Key.W)
            {
                await ViewModel.Canvas3d.MoveCloser();
            }

            if (e.Key == Key.S)
            {
                await ViewModel.Canvas3d.MoveFurther();
            }
        }
    }
}