using System.Windows;

using Graphal.VisualDebug.Abstractions.Logging;

namespace Graphal.VisualDebug.LogConsole
{
    public partial class LogConsoleView
    {
        public LogConsoleView()
        {
            InitializeComponent();
        }

        private void LogConsoleView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = e.NewValue as ILogConsoleViewModel;
            if (viewModel?.Entries == null)
            {
                return;
            }

            viewModel.Entries.CollectionChanged += (o, args) =>
            {
                ScrollViewer.ScrollToEnd();
            };
        }

        private void LogConsoleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToEnd();
        }
    }
}