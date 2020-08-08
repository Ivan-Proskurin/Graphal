using System.Drawing;

namespace Graphal.Engine.Abstractions.TwoD.Rendering
{
    public interface ICanvas2D
    {
        void Set(int x, int y, Color color);

        void Clear();

        void BeginDraw();

        void EndDraw();
    }
}