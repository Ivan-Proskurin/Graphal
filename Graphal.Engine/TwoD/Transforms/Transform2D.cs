using Graphal.Engine.Persistence.TwoD.Transforms;
using Graphal.Engine.TwoD.Geometry;

namespace Graphal.Engine.TwoD.Transforms
{
    public abstract class Transform2D
    {
        public abstract Vector2D Apply(Vector2D vector);

        public abstract Vector2D Invert(Vector2D vector);

        public abstract void Combine(Transform2D transform);

        public abstract Transform2Ds ToTransform2Ds();
    }
}