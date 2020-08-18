using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.Abstractions.IntersectBehaviours
{
    public interface IIntersectionFactory
    {
        IIntersectionBehaviour CreateBehaviour(Primitive2D primitive1, Primitive2D primitive2);
    }
}