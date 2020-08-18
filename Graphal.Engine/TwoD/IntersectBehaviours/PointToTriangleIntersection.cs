using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.TwoD.IntersectBehaviours
{
    public class PointToTriangleIntersection : IIntersectionBehaviour
    {
        private readonly Triangle2D _triangle;
        private readonly Point2D _point;

        public PointToTriangleIntersection(Triangle2D triangle, Point2D point)
        {
            _triangle = triangle;
            _point = point;
        }
        
        public bool Intersects()
        {
            return _triangle.Contains(_point.Vector);
        }
    }
}