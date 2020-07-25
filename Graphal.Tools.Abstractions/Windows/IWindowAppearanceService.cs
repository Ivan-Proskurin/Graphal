using System.Threading.Tasks;

namespace Graphal.Tools.Abstractions.Windows
{
    public interface IWindowAppearanceService
    {
        Task<WindowAppearance> LoadAsync(string name);

        Task SaveAsync(string name, WindowAppearance appearance);
    }
}