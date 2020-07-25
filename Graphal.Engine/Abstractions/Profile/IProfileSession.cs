using System;

namespace Graphal.Engine.Abstractions.Profile
{
    public interface IProfileSession : IDisposable
    {
        void LogWithPerformance(string text);

        TimeSpan Elapsed { get; }
    }
}