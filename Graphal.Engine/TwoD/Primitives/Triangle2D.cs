using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Primitives
{
    public class Triangle2D : Primitive2D
    {
        private readonly Vector2D _origV1;
        private readonly Vector2D _origV2;
        private readonly Vector2D _origV3;
        private readonly Color _color;

        private Vector2D _v1;
        private Vector2D _v2;
        private Vector2D _v3;

        private Line2D _line1;
        private Line2D _line2;
        private Line2D _line3;
        private int _test1;
        private int _test2;
        private int _test3;
        private EmbracingRect _rect;

        public Triangle2D(Vector2D v1, Vector2D v2, Vector2D v3, Color color)
        {
            _origV1 = v1;
            _origV2 = v2;
            _origV3 = v3;
            _color = color;
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
            UpdateGeometry();
        }

        public override Primitive2D Clone()
        {
            return new Triangle2D(_origV1, _origV2, _origV3, _color);
        }

        public override void Transform(Transform2D transform)
        {
            _v1 = transform.Apply(_origV1);
            _v2 = transform.Apply(_origV2);
            _v3 = transform.Apply(_origV3);
            UpdateGeometry();
        }

        public override void Render(ICanvas canvas)
        {
            for (var y = _rect.Top; y <= _rect.Bottom; y++)
            {
                for (var x = _rect.Left; x <= _rect.Right; x++)
                {
                    if (_line1.Test(x, y, _test1) && _line2.Test(x, y, _test2) && _line3.Test(x, y, _test3))
                    {
                        canvas.Set(x, y, _color);
                    }
                }
            }
        }

        private void UpdateGeometry()
        {
            _line1 = new Line2D(_v1, _v2);
            _line2 = new Line2D(_v2, _v3);
            _line3 = new Line2D(_v1, _v3);
            _test1 = _line1.Substitute(_v3.X, _v3.Y);
            _test2 = _line2.Substitute(_v1.X, _v1.Y);
            _test3 = _line3.Substitute(_v2.X, _v2.Y);
            _rect = EmbracingRect.Empty;
            _rect.ExtendBy(_v1.X, _v1.Y);
            _rect.ExtendBy(_v2.X, _v2.Y);
            _rect.ExtendBy(_v3.X, _v3.Y);
        }
    }
}