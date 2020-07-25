using System.IO;
using System.Threading.Tasks;
using Graphal.Tools.Storage.Abstractions;

namespace Graphal.Tools.Storage
{
    public class FileStorage : IFileStorage
    {
        public async Task<string> ReadAsStringAsync(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task WriteAsync(string path, string content)
        {
            var directoryInfo = Directory.GetParent(path);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(directoryInfo.FullName);
            }

            using (var writer = File.CreateText(path))
            {
                await writer.WriteAsync(content);
            }
        }
    }
}