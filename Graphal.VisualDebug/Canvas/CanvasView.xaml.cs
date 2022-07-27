using System;
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
        }

        private async void CanvasView_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void CanvasView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel?.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private async void CanvasView_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel == null) return;
            var (x, y) = e.GetClickPointAsDouble(this);
            await ViewModel.StartRotateAsync(x, y);
        }

        private async void CanvasView_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel == null) return;
            var (x, y) = e.GetClickPointAsDouble(this);
            await ViewModel.StopRotateAsync(x, y);
        }
        
        private async void CanvasView_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (ViewModel == null || e.RightButton != MouseButtonState.Pressed) return;
            var (x, y) = e.GetClickPointAsDouble(this);
            await ViewModel.ContinueRotateAsync(x, y);
        }

        private ICanvasViewModel3d ViewModel => this.GetViewModel<ICanvasViewModel3d>();

        private async void CanvasView_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var grade = e.Delta / 1000d;
            if (grade > 0)
                await ViewModel.MoveCloser(Math.Abs(grade));
            else
                await ViewModel.MoveFurther(Math.Abs(grade));
        }
    }
}