using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;
using Graphal.VisualDebug.Abstractions.Canvas;

using Color = System.Drawing.Color;

namespace Graphal.VisualDebug.Rendering
{
    public class BitmapCanvas : IBitmapCanvas
    {
        private readonly WriteableBitmap _bitmap;
        private EmbracingRect _dirtyRect = EmbracingRect.Empty;
        private bool _locked;

        public BitmapCanvas(IRenderingSettingsProvider renderingSettingsProvider)
        {
            _bitmap = InitializeBitmap(renderingSettingsProvider);
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public object Bitmap => _bitmap;

        public void SetSize(int width, int height)
        {
            if (width < 0 || width > _bitmap.PixelWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height < 0 || height > _bitmap.PixelHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
        }

        public void BeginDraw()
        {
            if (_bitmap == null)
            {
                throw new InvalidOperationException("Canvas is not ready");
            }

            if (_locked)
            {
                throw new InvalidOperationException("Bitmap is locked");
            }

            _bitmap.Lock();
            _locked = true;
        }

        public void EndDraw()
        {
            if (!_locked)
            {
                throw new InvalidOperationException("Bitmap is unlocked");
            }

            if (!_dirtyRect.IsEmpty)
            {
                var dirtyRectInt32 = new Int32Rect(_dirtyRect.Left, _dirtyRect.Top, _dirtyRect.Width, _dirtyRect.Height);
                _bitmap.AddDirtyRect(dirtyRectInt32);
                _dirtyRect.Clear();
            }

            _bitmap.Unlock();
            _locked = false;
        }

        public void Set(int x, int y, Color color)
        {
            CheckLocked();

            if (x < 0 || y < 0 || x >= _bitmap.PixelWidth || y >= _bitmap.PixelHeight)
            {
                return;
            }

            unsafe
            {
                // Get a pointer to the back buffer.
                var pBackBuffer = _bitmap.BackBuffer;

                // Find the address of the pixel to draw.
                pBackBuffer += y * _bitmap.BackBufferStride;
                pBackBuffer += x * 4;

                // Assign the color data to the pixel.
                *((int*) pBackBuffer) = color.ToArgb();
            }

            _dirtyRect.ExtendBy(x, y);
        }

        public void Clear()
        {
            CheckLocked();

            // Get a pointer to the back buffer.
            var pBackBuffer = _bitmap.BackBuffer;

            // Clear back buffer
            RtlZeroMemory(pBackBuffer, _bitmap.PixelWidth * _bitmap.PixelHeight * 4);

            // Set dirty rect to all canvas area
            _dirtyRect.SetTo(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
        }

        private static WriteableBitmap InitializeBitmap(IRenderingSettingsProvider renderingSettingsProvider)
        {
            var settings = renderingSettingsProvider.GetRenderingSettings();
            var bitmap = new WriteableBitmap(
                settings.ScreenWidth,
                settings.ScreenHeight,
                96,
                96,
                PixelFormats.Bgra32,
                null);
            return bitmap;
        }

        private void CheckLocked()
        {
            if (!_locked)
            {
                throw new InvalidOperationException("BeginDraw must be called before any output");
            }
        }
        
        [DllImport("kernel32.dll")]
        private static extern void RtlZeroMemory(IntPtr dst, int length);
    }
}