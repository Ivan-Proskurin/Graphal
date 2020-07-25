using System.Collections.Generic;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Rendering
{
    public class RenderingMap : IRenderingMap
    {
        private Vector2D _origin;
        private readonly Dictionary<Vector2D, RenderingFrame> _frames = new Dictionary<Vector2D, RenderingFrame>();

        public void Render(ICanvas canvas)
        {
            throw new System.NotImplementedException();
        }

        public void Transform(Transform2D transform)
        {
            _origin = transform.Apply(new Vector2D(0, 0));
        }

        public void Append(Primitive2D primitive)
        {
            throw new System.NotImplementedException();
        }

        private RenderingFrame GetOrCreateFrame(Vector2D origin)
        {
            return new RenderingFrame(origin);
        }

        private class RenderingFrame
        {
            private Vector2D _origin;
            private readonly List<Primitive2D> _primitives = new List<Primitive2D>();

            public RenderingFrame(Vector2D origin)
            {
                _origin = origin;
            }

            public void Append(Primitive2D primitive)
            {
                
            }

            public void Render(ICanvas canvas)
            {

            }

            public void Transform(Transform2D transform)
            {
                
            }
        }
    }
}