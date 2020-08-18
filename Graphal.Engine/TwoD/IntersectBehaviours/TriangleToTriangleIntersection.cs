using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.TwoD.IntersectBehaviours
{
    public class TriangleToTriangleIntersection : IIntersectionBehaviour
    {
        private readonly Triangle2D _triangle1;
        private readonly Triangle2D _triangle2;

        public TriangleToTriangleIntersection(Triangle2D triangle1, Triangle2D triangle2)
        {
            _triangle1 = triangle1;
            _triangle2 = triangle2;
        }

        public bool Intersects()
        {
            return _triangle1.IntersectsWith(_triangle2);
        }
    }
}