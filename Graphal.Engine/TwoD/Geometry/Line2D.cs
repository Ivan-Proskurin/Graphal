using System;

namespace Graphal.Engine.TwoD.Geometry
{
    public readonly struct Line2D
    {
        private readonly int _a;
        private readonly int _b;
        private readonly int _c;
        private readonly int _d1;
        private readonly int _d2;

        public Line2D(Vector2D v1, Vector2D v2)
        {
            _a = v2.Y - v1.Y;
            _b = v1.X - v2.X;
            _c = (v1.Y - v2.Y) * v2.X - v2.Y * (v1.X - v2.X);

            var sign = -Math.Sign(_a * _b);
            _d1 = (int)(_a * (v1.X - sign * 0.5) + _b * (v1.Y + 0.5) + _c);
            _d2 = (int)(_a * (v1.X + sign * 0.5) + _b * (v1.Y - 0.5) + _c);
            if (!(_d1 > _d2)) return;
            var d = _d1;
            _d1 = _d2;
            _d2 = d;
        }

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
    }
}