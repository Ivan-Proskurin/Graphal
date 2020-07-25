using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.Abstractions.TwoD.Rendering
{
    public interface IScene2D
    {
        void Append(Primitive2D primitive);

        void Render();

        void BeginShift(int x, int y);

        void Shift(int x, int y);

        void EndShift(int x, int y);

        Vector2D ToWorldCoordinates(Vector2D v);
    }
}