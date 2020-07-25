using System.Threading.Tasks;

namespace Graphal.Tools.Storage.Abstractions
{
    public interface IFileStorage
    {
        Task<string> ReadAsStringAsync(string path);

        Task WriteAsync(string path, string content);
    }
}