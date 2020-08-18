using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.TwoD.IntersectBehaviours
{
    public class IntersectionFactory : IIntersectionFactory
    {
        public IIntersectionBehaviour CreateBehaviour(Primitive2D primitive1, Primitive2D primitive2)
        {
            if (primitive1 is Point2D point1 && primitive2 is Point2D point2)
            {
                return new PointToPointIntersection(point1, point2);
            }

            if (primitive1 is Triangle2D triangle1 && primitive2 is Triangle2D trinagle2)
            {
                return new TriangleToTriangleIntersection(triangle1, trinagle2);
            }

            if (primitive1 is Triangle2D triangle2 && primitive2 is Point2D point)
            {
                return new PointToTriangleIntersection(triangle2, point);
            }

            if (primitive1 is Point2D point2D && primitive2 is Triangle2D triangle2D)
            {
                return new PointToTriangleIntersection(triangle2D, point2D);
            }

            throw new System.NotImplementedException();
        }
    }
}