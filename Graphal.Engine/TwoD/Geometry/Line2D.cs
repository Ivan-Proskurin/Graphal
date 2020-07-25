using System;

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
        private readonly int _dx1;
        private readonly int _dx2;

        public Line2D(Vector2D v1, Vector2D v2)
        {
            _v1 = v1;
            _v2 = v2;
            _a = v2.Y - v1.Y;
            _b = v1.X - v2.X;
            _c = (v1.Y - v2.Y) * v2.X - v2.Y * (v1.X - v2.X);

            var sign = -Math.Sign(_a * _b);
            _d1 = 0;
            _d2 = 0;
            // _d1 = (int)(_a * (v1.X - sign * 0.5) + _b * (v1.Y + 0.5) + _c);
            // _d2 = (int)(_a * (v1.X + sign * 0.5) + _b * (v1.Y - 0.5) + _c);
            _dx1 = _a != 0 ? (int)Math.Round(v1.X - sign * 0.5 + (_b * (v1.Y + 0.5) + _c) / _a) : 0;
            _dx2 = _a != 0 ? (int)Math.Round(v1.X + sign * 0.5 + (_b * (v1.Y - 0.5) + _c) / _a) : 0;
            // if (_d1 > _d2)
            // {
            //     var d = _d1;
            //     _d1 = _d2;
            //     _d2 = d;
            // }

            if (_dx1 > _dx2)
            {
                var dx = _dx1;
                _dx1 = _dx2;
                _dx2 = dx;
            }

            if (_b < 0)
            {
                var dx = _dx1;
                _dx1 = _dx2;
                _dx2 = dx;
            }
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

        public int IntersectYToLeft(int y)
        {
            return (_dx1 - _c - _b * y) / _a;
        }

        public int IntersectYToRight(int y)
        {
            return (_dx2 - _c - _b * y) / _a;
        }

        public bool IsHorizontal()
        {
            return _a == 0;
        }
    }
}