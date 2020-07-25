using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;

namespace Graphal.Engine.Tests.Mocks
{
    public class CanvasMock : ICanvas
    {
        private Color[,] _canvas;
        
        public CanvasMock(int width, int height, Color color)
        {
            _canvas = new Color[width, height];
            FillWith(color);
        }

        public int Width => _canvas.GetLength(0);

        public int Height => _canvas.GetLength(1);

        public Color this[int x, int y] => _canvas[x, y];

        public void BeginDraw()
        {
        }

        public void EndDraw()
        {
        }

        public void Set(int x, int y, Color color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }

            _canvas[x, y] = color;
        }

        public void Clear()
        {
            _canvas = new Color[Width, Height];
        }

        public void FillWith(Color color)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _canvas[x, y] = color;
                }
            }
        }
    }
}