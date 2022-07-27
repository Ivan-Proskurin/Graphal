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
        private readonly object _owner;

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

        public Triangle2D(Vector2D v1, Vector2D v2, Vector2D v3, Color color, object owner = null)
        {
            _origV1 = v1;
            _origV2 = v2;
            _origV3 = v3;
            _color = color;
            _owner = owner;
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
            var y = _v1.Y;
            using (var line1Enumerator = _line1.Render(canvas, _color).GetEnumerator())
            using (var line2Enumerator = _line2.Render(canvas, _color).GetEnumerator())
            using (var line3Enumerator = _line3.Render(canvas, _color).GetEnumerator())
            {
                var contrEnumerator = line1Enumerator;
                while (line3Enumerator.MoveNext())
                {
                    if (!contrEnumerator.MoveNext())
                    {
                        contrEnumerator = line2Enumerator;
                        contrEnumerator.MoveNext();
                    }

                    var x1 = contrEnumerator.Current;
                    var x2 = line3Enumerator.Current;
                    if (x1 > x2)
                    {
                        var d = x1;
                        x1 = x2;
                        x2 = d;
                    }
                    
                    for (var x = x1; x <= x2; x++)
                    {
                        canvas.Set(x, y, _color);
                    }

                    y++;
                }

                while (contrEnumerator.MoveNext())
                {
                    
                }
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