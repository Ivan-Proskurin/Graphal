using System.Drawing;

namespace Graphal.Engine.TwoD.Geometry
{
    public struct EmbracingRect
    {
        public int Left { get; private set; }

        public int Top { get; private set; }

        public int Right { get; private set; }

        public int Bottom { get; private set; }

        public int Width => Right - Left + 1;

        public int Height => Bottom - Top + 1;

        public void ExtendBy(int x, int y)
        {
            if (IsEmpty)
            {
                Left = x;
                Top = y;
                Right = x;
                Bottom = y;
                return;
            }

            if (x < Left)
            {
                Left = x;
            }

            if (y < Top)
            {
                Top = y;
            }

            if (x > Right)
            {
                Right = x;
            }

            if (y > Bottom)
            {
                Bottom = y;
            }
        }

        public void SetTo(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Right = x + width - 1;
            Bottom = y + height - 1;
        }

        public void Clear()
        {
            Left = 0;
            Top = 0;
            Right = -1;
            Bottom = -1;
        }

        public bool IsEmpty => Left > Right || Top > Bottom;

        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Width, Height);
        }

        public static EmbracingRect Empty {
            get
            {
                var rect = new EmbracingRect();
                rect.Clear();
                return rect;
            }
        }
    }
}