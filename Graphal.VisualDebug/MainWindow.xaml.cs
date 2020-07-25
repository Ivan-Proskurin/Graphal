using System;
using Graphal.VisualDebug.Abstractions;

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
    }
}