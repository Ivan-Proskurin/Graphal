using System.Collections.Generic;
using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;

namespace Graphal.Engine.TwoD.Geometry
{
    public readonly struct Line2D
    {
        private readonly Vector2D _v1;
        private readonly Vector2D _v2;
        private readonly int _a;
        private readonly int _b;
        private readonly int _c;
        private readonly int _d1;
        private readonly int _d2;

        public Line2D(Vector2D v1, Vector2D v2)
        {
            _v1 = v1;
            _v2 = v2;
            _a = v2.Y - v1.Y;
            _b = v1.X - v2.X;
            _c = (v1.Y - v2.Y) * v2.X - v2.Y * (v1.X - v2.X);

            _d1 = 0;
            _d2 = 0;
        }

        public IEnumerable<int> Render(ICanvas2D canvas, Color color) 
        {
            // sorting X
            var x1 = X1;
            var x2 = X2;
            if (x1 > x2)
            {
                var dx = x1;
                x1 = x2;
                x2 = dx;
            }

            // sorting Y
            var y1 = Y1;
            var y2 = Y2;
            if (y1 > y2)
            {
                var dy = y1;
                y1 = y2;
                y2 = dy;
            }

            if (_a == 0)
            {
                // horizontal
                for (var x = x1; x <= x2; x++)
                {
                    canvas.Set(x, Y1, color);
                }

                yield return x1;
            }
            else if (_b == 0)
            {
                // vertical
                for (var y = y1; y <= y2; y++)
                {
                    canvas.Set(X1, y, color);
                    yield return X1;
                }
            }
            else
            {
                var d = _a / _b;
                var w = _b / _a;
                // steep sloping up/down
                if (d >= 1 || d <= -1)
                {
                    for (var y = y1; y <= y2; y++)
                    {
                        var x = GetXByY(y);
                        canvas.Set(x, y, color);
                        yield return x;
                    }
                }
                // slow sloping up
                else if (w >= 1)
                {
                    var currentY = (int?)null;
                    for (var x = x2; x >= x1; x--)
                    {
                        var y = GetYByX(x);
                        canvas.Set(x, y, color);
                        if (currentY != y)
                        {
                            currentY = y;
                            yield return x;
                        }
                    }
                }
                // slow slopping down
                else if (w <= -1)
                {
                    var currentY = (int?)null;
                    for (var x = x1; x <= x2; x++)
                    {
                        var y = GetYByX(x);
                        canvas.Set(x, y, color);
                        if (currentY != y)
                        {
                            currentY = y;
                            yield return x;
                        }
                    }
                }
            }
        }

        private int GetXByY(int y)
        {
            return (-_b * y - _c) / _a;
        }

        private int GetYByX(int x)
        {
            return (-_a * x - _c) / _b;
        }

        public int X1 => _v1.X;

        public int X2 => _v2.X;

        public int Y1 => _v1.Y;

        public int Y2 => _v2.Y;

        public int Substitute(int x, int y)
        {
            return _a * x + _b * y + _c;
        }

        public bool Test(int x, int y, int value)
        {
            var test = Substitute(x, y);

            if (value == 0)
            {
                return test >= _d1 && test <= _d2;
            }

            if (value < 0)
            {
                return test <= _d2;
            }

            return test >= _d1;
        }

        public bool IntersectsWith(Line2D other)
        {
            var a1 = this._a;
            var b1 = this._b;
            var c1 = this._c;
            var a2 = other._a;
            var b2 = other._b;
            var c2 = other._c;

            var del = a2 * b1 - a1 * b2;
            if (del == 0)
            {
                return false;
            }

            var x = (b2 * c1 - b1 * c2) / del;
            var y = (-a1 * x - c1) / b1;
            return x >= _v1.X && x <= _v2.X && y >= _v1.Y && y <= _v2.Y &&
                   x >= other._v1.X && x <= other._v2.X && y >= other._v1.Y && y <= other._v2.Y;
        }
    }
}