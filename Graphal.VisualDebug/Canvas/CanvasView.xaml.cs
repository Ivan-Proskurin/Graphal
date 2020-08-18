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

        private async void CanvasView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                await ViewModel.InitializeAsync();
            }
        }

        private async void CanvasView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel == null) return;
            var clickPoint = e.GetPosition(this);
            // await ViewModel.BeginShiftAsync((int)clickPoint.X, (int)clickPoint.Y);
        }

        private async void CanvasView_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (ViewModel == null || e.LeftButton != MouseButtonState.Pressed) return;
            var clickPoint = e.GetPosition(this);
            // await ViewModel.ShiftAsync((int)clickPoint.X, (int)clickPoint.Y);
        }

        private async void CanvasView_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // var clickPoint = e.GetPosition(this);
            // ViewModel?.SetPoint((int)clickPoint.X, (int)clickPoint.Y);
            if (ViewModel == null) return;
            var clickPoint = e.GetPosition(this);
            // await ViewModel.EndShiftAsync((int)clickPoint.X, (int)clickPoint.Y);
        }

        private void CanvasView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel?.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void CanvasView_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel == null) return;
            var clickPoint = e.GetPosition(this);
            // ViewModel.BeginShift((int)clickPoint.X, (int)clickPoint.Y);
        }

        private void CanvasView_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel == null) return;
            var clickPoint = e.GetPosition(this);
            // ViewModel.EndShift((int)clickPoint.X, (int)clickPoint.Y);
        }

        private ICanvasViewModel3d ViewModel => this.GetViewModel<ICanvasViewModel3d>();
    }
}