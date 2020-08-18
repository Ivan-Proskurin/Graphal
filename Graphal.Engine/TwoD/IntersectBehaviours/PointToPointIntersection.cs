using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.TwoD.IntersectBehaviours
{
    public class PointToPointIntersection : IIntersectionBehaviour
    {
        private readonly Point2D _point1;
        private readonly Point2D _point2;

        public PointToPointIntersection(Point2D point1, Point2D point2)
        {
            _point1 = point1;
            _point2 = point2;
        }

        public bool Intersects()
        {
            return _point1.Vector == _point2.Vector;
        }
    }
}