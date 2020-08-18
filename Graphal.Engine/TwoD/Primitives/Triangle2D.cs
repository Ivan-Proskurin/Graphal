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

        private int _test1;
        private int _test2;
        private int _test3;

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

        public override void Render(ICanvas2D canvas)
        {
            if (_v2.X < _v3.X)
            {
                RenderBetween(_v1, _v2, _line1, _line3, canvas);
                RenderBetween(_v2, _v3, _line2, _line3, canvas);
            }
            else
            {
                RenderBetween(_v1, _v2, _line3, _line1, canvas);
                RenderBetween(_v2, _v3, _line3, _line2, canvas);
            }
        }

        private void RenderBetween(Vector2D v1, Vector2D v2, Line2D line1, Line2D line2, ICanvas2D canvas)
        {
            var y1 = v1.Y;
            var y2 = v2.Y;

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

        private void RenderStroke(int x1, int x2, int y, ICanvas2D canvas)
        {
            if (x1 > x2)
            {
                var d = x1;
                x1 = x2;
                x2 = d;
            }

            if (x1 < _rect.Left)
            {
                x1 = _rect.Left;
            }

            if (x2 > _rect.Right)
            {
                x2 = _rect.Right;
            }
            
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

        public bool Contains(Vector2D v)
        {
            return v.X >= _rect.Left &&
                   v.X <= _rect.Right &&
                   v.Y >= _rect.Top &&
                   v.Y <= _rect.Bottom;
        }

        public bool IntersectsWith(Triangle2D other)
        {
            if (this.IsInside(other._v1) || this.IsInside(other._v2) || this.IsInside(other._v3) ||
                other.IsInside(this._v1) || other.IsInside(this._v2) || other.IsInside(this._v3))
            {
                return true;
            }

            if (_line1.IntersectsWith(other._line1))
            {
                return true;
            }
            
            if (_line1.IntersectsWith(other._line2))
            {
                return true;
            }
            
            if (_line1.IntersectsWith(other._line3))
            {
                return true;
            }
            
            if (_line2.IntersectsWith(other._line1))
            {
                return true;
            }
            
            if (_line2.IntersectsWith(other._line2))
            {
                return true;
            }
            
            if (_line2.IntersectsWith(other._line3))
            {
                return true;
            }
            
            if (_line3.IntersectsWith(other._line1))
            {
                return true;
            }
            
            if (_line3.IntersectsWith(other._line2))
            {
                return true;
            }
            
            if (_line3.IntersectsWith(other._line3))
            {
                return true;
            }

            return false;
        }

        public bool IsInside(Vector2D v)
        {
            return _line1.Test(v.X, v.Y, _test1) && _line2.Test(v.X, v.Y, _test2) && _line3.Test(v.X, v.Y, _test3);
        }

        private void UpdateGeometry()
        {
            Vector2D.SortByY(ref _v1, ref _v2, ref _v3);
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