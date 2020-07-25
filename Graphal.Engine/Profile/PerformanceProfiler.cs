using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.Profile;

namespace Graphal.Engine.Profile
{
    public class PerformanceProfiler : IPerformanceProfiler
    {
        private readonly ILogger _logger;

        public PerformanceProfiler(ILogger logger)
        {
            _logger = logger;
        }

        public IProfileSession CreateSession()
        {
            return new ProfileSession(elapsed => $"{elapsed.TotalMilliseconds} ms", _logger);
        }
    }
}