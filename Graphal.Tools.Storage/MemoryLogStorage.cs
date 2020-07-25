using System.Collections.Generic;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Tools.Storage.Abstractions;

namespace Graphal.Tools.Storage
{
    public class MemoryLogStorage : ILogStorage
    {
        private readonly object _syncRoot = new object();
        private readonly List<LogEntry> _entries = new List<LogEntry>();

        public void Append(LogEntry entry)
        {
            lock (_syncRoot)
            {
                _entries.Add(entry);
            }
        }

        public LogEntry[] Load()
        {
            lock (_syncRoot)
            {
                return _entries.ToArray();
            }
        }
    }
}