using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.VisualDebug.Abstractions.Canvas;

namespace Graphal.VisualDebug.Rendering
{
    public class BitmapOutputDevice : IOutputDevice, IBitmapSource
    {
        private readonly WriteableBitmap _bitmap;

        public BitmapOutputDevice(IRenderingSettingsProvider renderingSettingsProvider)
        {
            _bitmap = InitializeBitmap(renderingSettingsProvider);
        }

        public void Lock()
        {
            InvokeOnDispatcher(() => _bitmap.Lock());
        }

        public void MoveBuffer(int[] buffer)
        {
            InvokeOnDispatcher(() =>
            {
                if (buffer.Length > _bitmap.PixelWidth * _bitmap.PixelHeight)
                {
                    throw new InvalidOperationException("Passed buffer is greater then output bitmap");
                }

                var bitmapBuffer = _bitmap.BackBuffer;
                Marshal.Copy(buffer, 0, bitmapBuffer, buffer.Length);
            });
        }

        public void AddDirtyRect(Rectangle dirtyRect)
        {
            InvokeOnDispatcher(() =>
            {
                var rect = new Int32Rect(dirtyRect.Left, dirtyRect.Top, dirtyRect.Width, dirtyRect.Height);
                _bitmap.AddDirtyRect(rect);
            });
        }

        public void Unlock()
        {
            InvokeOnDispatcher(() => _bitmap.Unlock());
        }

        public object Bitmap => _bitmap;
        
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

        private void InvokeOnDispatcher(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}