using System;
using System.Diagnostics;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.Profile;

namespace Graphal.Engine.Profile
{
    public class ProfileSession : IProfileSession
    {
        private readonly Func<TimeSpan, string> _logEntryFormat;
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch;

        public ProfileSession(Func<TimeSpan, string> logEntryFormat, ILogger logger)
        {
            _logEntryFormat = logEntryFormat;
            _logger = logger;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
        }

        public void LogWithPerformance(string text)
        {
            _logger.Info($"{text} > {_logEntryFormat(_stopwatch.Elapsed)}");
        }

        public TimeSpan Elapsed => _stopwatch.Elapsed;
    }
}