namespace Graphal.Engine.Abstractions.Logging
{
    public interface ILogObserver
    {
        event LogEntryAddedEventHandler LogEntryAdded;
    }

    public delegate void LogEntryAddedEventHandler(ILogObserver sender, LogEntryAddedEventArgs e);
}