using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Helpers;

namespace Graphal.VisualDebug.Canvas
{
    public partial class CanvasView
    {
        public CanvasView()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(Img, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(Img, EdgeMode.Aliased);
        }

        private void CanvasView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Initialize();
        }

        private void CanvasView_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition(this);
            ViewModel?.SetPoint((int)clickPoint.X, (int)clickPoint.Y);
        }

        private void CanvasView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel?.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void CanvasView_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition(this);
            ViewModel?.BeginShift((int)clickPoint.X, (int)clickPoint.Y);
        }

        private void CanvasView_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clickPoint = e.GetPosition(this);
            ViewModel?.EndShift((int)clickPoint.X, (int)clickPoint.Y);
        }

        private void CanvasView_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var clickPoint = e.GetPosition(this);
                ViewModel?.Shift((int)clickPoint.X, (int)clickPoint.Y);
            }
        }

        private ICanvasViewModel ViewModel => this.GetViewModel<ICanvasViewModel>();
    }
}