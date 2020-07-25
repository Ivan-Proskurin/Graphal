using System.Threading.Tasks;

using Graphal.Engine.Persistence.TwoD;

namespace Graphal.Tools.Abstractions.Persistence
{
    public interface IScenePersistenceService
    {
        Task<Scene2Ds> LoadScene2DAsync();

        Task SaveScene2DAsync(Scene2Ds scene);
    }
}