using System.Collections.ObjectModel;
using System.Linq;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Tools.Storage.Abstractions;
using Graphal.VisualDebug.Abstractions.Logging;
using Graphal.VisualDebug.Abstractions.Wrappers;
using Graphal.VisualDebug.ViewModels.Helpers;

namespace Graphal.VisualDebug.ViewModels.Logging
{
    public class LogConsoleViewModel : ILogConsoleViewModel
    {
        private readonly ILogStorage _logStorage;
        private readonly ILogObserver _logObserver;
        private readonly IDispatcherWrapper _dispatcherWrapper;

        public LogConsoleViewModel(ILogStorage logStorage, ILogObserver logObserver, IDispatcherWrapper dispatcherWrapper)
        {
            _logStorage = logStorage;
            _logObserver = logObserver;
            _dispatcherWrapper = dispatcherWrapper;
        }

        public ObservableCollection<ILogEntryViewModel> Entries { get; private set; }

        public void Initialize()
        {
            var entries = _logStorage.Load();
            Entries = entries.Select(x => x.ToLogEntryViewModel()).ToObservableCollection();
            _logObserver.LogEntryAdded += (sender, args) =>
            {
                _dispatcherWrapper.Invoke(() => Entries.Add(args.Entry.ToLogEntryViewModel()));
            };
        }
    }
}