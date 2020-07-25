using System.Collections.ObjectModel;
using System.Linq;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Tools.Storage.Abstractions;
using Graphal.VisualDebug.Abstractions.Logging;
using Graphal.VisualDebug.ViewModels.Helpers;

namespace Graphal.VisualDebug.ViewModels.Logging
{
    public class LogConsoleViewModel : ILogConsoleViewModel
    {
        private readonly ILogStorage _logStorage;
        private readonly ILogObserver _logObserver;

        public LogConsoleViewModel(ILogStorage logStorage, ILogObserver logObserver)
        {
            _logStorage = logStorage;
            _logObserver = logObserver;
        }

        public ObservableCollection<ILogEntryViewModel> Entries { get; private set; }

        public void Initialize()
        {
            var entries = _logStorage.Load();
            Entries = entries.Select(x => x.ToLogEntryViewModel()).ToObservableCollection();
            _logObserver.LogEntryAdded += (sender, args) => Entries.Add(args.Entry.ToLogEntryViewModel());
        }
    }
}