namespace Graphal.Tools.Abstractions.Application
{
    public interface IApplicationStandardPaths
    {
        string UserApplicationSettings { get; }

        string UserLocalStorage { get; }
    }
}