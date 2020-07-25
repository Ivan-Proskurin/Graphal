using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Persistence.TwoD.Primitives;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Primitives
{
    public class Triangle2D : Primitive2D
    {
        private readonly Vector2D _origV1;
        private readonly Vector2D _origV2;
        private readonly Vector2D _origV3;
        private Color _color;

        private Vector2D _v1;
        private Vector2D _v2;
        private Vector2D _v3;

        private Line2D _line1;
        private Line2D _line2;
        private Line2D _line3;
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
            if (_v2.X < _v3.X)
            {
                RenderBetween(_v1.Y, _v2.Y, _line1, _line3, canvas);
                RenderBetween(_v2.Y, _v3.Y, _line2, _line3, canvas);
            }
            else
            {
                RenderBetween(_v1.Y, _v2.Y, _line3, _line1, canvas);
                RenderBetween(_v2.Y, _v3.Y, _line3, _line2, canvas);
            }
        }

        private void RenderBetween(int y1, int y2, Line2D line1, Line2D line2, ICanvas canvas)
        {
            if (line1.IsHorizontal())
            {
                RenderStroke(line1.X1, line1.X2, line1.Y1, canvas);
                return;
            }

            if (line2.IsHorizontal())
            {
                RenderStroke(line2.X1, line2.X2, line2.Y1, canvas);
                return;
            }

            for (var y = y1; y <= y2; y++)
            {
                var x1 = line1.IntersectYToLeft(y);
                if (x1 < _rect.Left)
                {
                    x1 = _rect.Left;
                }

                var x2 = line2.IntersectYToRight(y);
                if (x2 > _rect.Right)
                {
                    x2 = _rect.Right;
                }

                if (x1 == x2)
                {
                    canvas.Set(x1, y, _color);
                }
                else
                {
                    RenderStroke(x1, x2, y, canvas);
                }
            }
        }

        private void RenderStroke(int x1, int x2, int y, ICanvas canvas)
        {
            for (var x = x1; x <= x2; x++)
            {
                canvas.Set(x, y, _color);
            }
        }
        
        public override Primitive2Ds ToPrimitive2Ds()
        {
            return new Triangle2Ds
            {
                V1 = _origV1.ToVector2Ds(),
                V2 = _origV2.ToVector2Ds(),
                V3 = _origV3.ToVector2Ds(),
                Color = _color.ToArgb(),
            };
        }

        private void UpdateGeometry()
        {
            Vector2D.SortByY(ref _v1, ref _v2, ref _v3);
            _line1 = new Line2D(_v1, _v2);
            _line2 = new Line2D(_v2, _v3);
            _line3 = new Line2D(_v1, _v3);
            _rect = EmbracingRect.Empty;
            _rect.ExtendBy(_v1.X, _v1.Y);
            _rect.ExtendBy(_v2.X, _v2.Y);
            _rect.ExtendBy(_v3.X, _v3.Y);
        }
    }
}