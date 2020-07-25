using System.IO;
using System.Threading.Tasks;

using Graphal.Engine.Persistence.TwoD;
using Graphal.Tools.Abstractions.Application;
using Graphal.Tools.Abstractions.Persistence;
using Graphal.Tools.Abstractions.Serialization;
using Graphal.Tools.Storage.Abstractions;

namespace Graphal.Tools.Services.Persistence
{
    public class ScenePersistenceService : IScenePersistenceService
    {
        private readonly IApplicationStandardPaths _applicationStandardPaths;
        private readonly IXmlSerializationService _xmlSerializationService;
        private readonly IFileStorage _fileStorage;

        public ScenePersistenceService(
            IApplicationStandardPaths applicationStandardPaths,
            IXmlSerializationService xmlSerializationService,
            IFileStorage fileStorage)
        {
            _applicationStandardPaths = applicationStandardPaths;
            _xmlSerializationService = xmlSerializationService;
            _fileStorage = fileStorage;
        }

        public async Task<Scene2Ds> LoadScene2DAsync()
        {
            try
            {
                var path = GetScene2DPath();
                var fileContent = await _fileStorage.ReadAsStringAsync(path);
                return _xmlSerializationService.Deserialize<Scene2Ds>(fileContent);
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

        public Task SaveScene2DAsync(Scene2Ds scene)
        {
            var path = GetScene2DPath();
            var fileContent = _xmlSerializationService.Serialize(scene);
            return _fileStorage.WriteAsync(path, fileContent);
        }

        private string GetScene2DPath()
        {
            return Path.Combine(_applicationStandardPaths.UserLocalStorage, "scene2d.xml");
        }
    }
}