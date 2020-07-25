using Graphal.Engine.Abstractions.Logging;

namespace Graphal.Tools.Storage.Abstractions
{
    public interface ILogStorage
    {
        void Append(LogEntry entry);

        LogEntry[] Load();
    }
}