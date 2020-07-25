using Graphal.Engine.TwoD.Primitives;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.Abstractions.TwoD.Rendering
{
    public interface IRenderingMap
    {
        void Render(ICanvas canvas);

        void Transform(Transform2D transform);

        void Append(Primitive2D primitive);
    }
}