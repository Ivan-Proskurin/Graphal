using System.IO;
using System.Threading.Tasks;
using Graphal.Tools.Abstractions.Application;
using Graphal.Tools.Abstractions.Serialization;
using Graphal.Tools.Abstractions.Windows;
using Graphal.Tools.Storage.Abstractions;

namespace Graphal.Tools.Services.Windows
{
    public class WindowAppearanceService : IWindowAppearanceService
    {
        private const string AppearanceFolderName = "Appearance";

        private readonly IApplicationInfo _applicationInfo;
        private readonly IApplicationStandardPaths _applicationStandardPaths;
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileStorage _fileStorage;

        public WindowAppearanceService(
            IApplicationInfo applicationInfo,
            IApplicationStandardPaths applicationStandardPaths,
            IJsonSerializationService jsonSerializationService,
            IFileStorage fileStorage)
        {
            _applicationInfo = applicationInfo;
            _applicationStandardPaths = applicationStandardPaths;
            _jsonSerializationService = jsonSerializationService;
            _fileStorage = fileStorage;
        }

        public async Task<WindowAppearance> LoadAsync(string name)
        {
            try
            {
                var path = GetWindowAppearanceFilePath(name);
                var fileContent = await _fileStorage.ReadAsStringAsync(path);
                return _jsonSerializationService.Deserialize<WindowAppearance>(fileContent);
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public Task SaveAsync(string name, WindowAppearance appearance)
        {
            var path = GetWindowAppearanceFilePath(name);
            var content = _jsonSerializationService.Serialize(appearance);
            return _fileStorage.WriteAsync(path, content);
        }

        private string GetWindowAppearanceFilePath(string name)
        {
            return Path.Combine(
                _applicationStandardPaths.UserApplicationSettings,
                _applicationInfo.ApplicationName,
                AppearanceFolderName,
                $"{name}.json");
        }
    }
}