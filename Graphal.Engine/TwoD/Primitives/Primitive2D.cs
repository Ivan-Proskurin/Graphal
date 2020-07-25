using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Primitives
{
    public abstract class Primitive2D
    {
        public abstract Primitive2D Clone();

        public abstract void Transform(Transform2D transform);

        public abstract void Render(ICanvas canvas);
    }
}