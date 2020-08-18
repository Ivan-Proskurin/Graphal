using System.Collections.Generic;
using System.Linq;

using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.TwoD.Rendering
{
    public class RenderingFrame
    {
        private readonly IIntersectionFactory _intersectionFactory;
        private readonly List<Primitive2D> _primitives = new List<Primitive2D>();

        public RenderingFrame(List<Primitive2D> primitives, IIntersectionFactory intersectionFactory)
        {
            _intersectionFactory = intersectionFactory;
            _primitives = primitives;
        }

        public bool TryAppend(Primitive2D primitive)
        {
            if (!_primitives.Any())
            {
                _primitives.Add(primitive);
                return true;
            }

            foreach (var primitive2D in _primitives)
            {
                var intersection = _intersectionFactory.CreateBehaviour(primitive2D, primitive);
                if (intersection.Intersects())
                {
                    _primitives.Add(primitive);
                    return true;
                }
            }

            return false;
        }

        public void Render(ICanvas2D canvas)
        {
            foreach (var primitive in _primitives)
            {
                primitive.Render(canvas);
            }
        }

        public int Capacity => _primitives.Count;
    }
}