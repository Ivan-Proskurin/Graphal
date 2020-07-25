using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Primitives
{
    public class Edge2D : Primitive2D
    {
        private readonly Vector2D _originalV1;
        private readonly Vector2D _originalV2;
        private readonly Color _color;

        private Vector2D _v1;
        private Vector2D _v2;
        private Line2D _line;
        private EmbracingRect _rect;

        public Edge2D(Vector2D v1, Vector2D v2, Color color)
        {
            _originalV1 = v1;
            _originalV2 = v2;
            _v1 = v1;
            _v2 = v2;
            _color = color;
            UpdateGeometry();
        }

        public override Primitive2D Clone()
        {
            return new Edge2D(_originalV1, _originalV2, _color);
        }

        public override void Transform(Transform2D transform)
        {
            throw new System.NotImplementedException();
        }

        public override void Render(ICanvas canvas)
        {
            for (var y = _rect.Top; y <= _rect.Bottom; y++)
            {
                for (var x = _rect.Left; x <= _rect.Right; x++)
                {
                    if (_line.Test(x, y, 0))
                    {
                        canvas.Set(x, y, _color);
                    }
                }
            }
        }

        private void UpdateGeometry()
        {
            _line = new Line2D(_v1, _v2);
            _rect = EmbracingRect.Empty;
            _rect.ExtendBy(_v1.X, _v1.Y);
            _rect.ExtendBy(_v2.X, _v2.Y);
        }
    }
}