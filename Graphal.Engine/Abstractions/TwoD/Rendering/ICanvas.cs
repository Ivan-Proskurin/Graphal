using System.Drawing;

namespace Graphal.Engine.Abstractions.TwoD.Rendering
{
    public interface ICanvas
    {
        int Width { get; }

        int Height { get; }

        void BeginDraw();

        void EndDraw();

        void Set(int x, int y, Color color);

        void Clear();
    }
}