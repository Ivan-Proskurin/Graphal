namespace Graphal.Engine.Abstractions.Profile
{
    public interface IPerformanceProfiler
    {
        IProfileSession CreateSession();
    }
}