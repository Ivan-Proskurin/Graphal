using System;
using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;

namespace Graphal.Engine.TwoD.Rendering
{
    public class Canvas2D : ICanvas2D
    {
        private readonly IOutputDevice _outputDevice;
        private int _width;
        private int _height;
        private int[] _buffer;
        private EmbracingRect _dirtyRect = EmbracingRect.Empty;
        
        public Canvas2D(
            IRenderingSettingsProvider renderingSettingsProvider,
            IOutputDevice outputDevice)
        {
            _outputDevice = outputDevice;
            Initialize(renderingSettingsProvider);
        }

        public void Set(int x, int y, Color color)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return;
            }

            var index = y * _width + x;
            _buffer[index] = color.ToArgb();
            _dirtyRect.ExtendBy(x, y);
        }

        public void Clear()
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            _dirtyRect.SetTo(0, 0, _width, _height);
        }

        private bool _drawing;
        public void BeginDraw()
        {
            if (_drawing)
            {
                throw new InvalidOperationException("Already drawing");
            }

            _drawing = true;
            _dirtyRect.Clear();
        }

        public void EndDraw()
        {
            _drawing = false;
            if (_dirtyRect.IsEmpty)
            {
                return;
            }

            _outputDevice.Lock();
            try
            {
                _outputDevice.MoveBuffer(_buffer);
                _outputDevice.AddDirtyRect(_dirtyRect.ToRectangle());
            }
            finally
            {
                _outputDevice.Unlock();
            }
        }

        private void Initialize(IRenderingSettingsProvider renderingSettingsProvider)
        {
            var settings = renderingSettingsProvider.GetRenderingSettings();
            _width = settings.ScreenWidth;
            _height = settings.ScreenHeight;
            _buffer = new int[_height * _width];
        }
    }
}