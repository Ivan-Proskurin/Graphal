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

            if (e.Key == Key.Left)
            {
                await ViewModel.Canvas3d.RotateLeftAsync();
            }

            if (e.Key == Key.Right)
            {
                await ViewModel.Canvas3d.RotateRightAsync();
            }

            if (e.Key == Key.Up)
            {
                await ViewModel.Canvas3d.RotateUpAsync();
            }

            if (e.Key == Key.Down)
            {
                await ViewModel.Canvas3d.RotateDownAsync();
            }

            if (e.Key == Key.W)
            {
                await ViewModel.Canvas3d.MoveCloser();
            }

            if (e.Key == Key.S)
            {
                await ViewModel.Canvas3d.MoveFurther();
            }
        }

        private async void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (ViewModel == null) return;

            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                case Key.Left:
                case Key.Right:
                    await ViewModel.Canvas3d.StopRotationAsync();
                    break;
            }
        }
    }
}