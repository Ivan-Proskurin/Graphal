using System;
using System.Drawing;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.ThreeD.Rendering;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;
using Graphal.Engine.TwoD.Geometry;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Wrappers;

namespace Graphal.VisualDebug.ViewModels.Canvas
{
    public class CanvasViewModel3d : ICanvasViewModel3d
    {
        private readonly IBitmapSource _bitmapSource;
        private readonly IScene3D _scene3D;
        private readonly ILogger _logger;
        private readonly IDispatcherWrapper _dispatcherWrapper;

        public CanvasViewModel3d(
            IBitmapSource bitmapSource,
            IScene3D scene3D,
            ILogger logger,
            IDispatcherWrapper dispatcherWrapper)
        {
            _bitmapSource = bitmapSource;
            _scene3D = scene3D;
            _logger = logger;
            _dispatcherWrapper = dispatcherWrapper;
        }

        public object ImageSource => _bitmapSource.Bitmap;

        public void Resize(int width, int height)
        {
        }

        public async Task InitializeAsync()
        {
            await _scene3D.InitializeAsync();
            _scene3D.FpsChanged += Scene3DOnFpsChanged;
            // CreateUmbrellaFigure();
            CreateSemiPetalUmbrellaFigure();
            await _scene3D.RenderAsync();
        }

        private void Scene3DOnFpsChanged(object sender, FpsChangedArgs e)
        {
            _dispatcherWrapper.Invoke(() => _logger.Info($"FPS: {e.Fps}"));
        }

        public Task RotateLeftAsync()
        {
            return _scene3D.RotateXZAsync(-2);
        }

        public Task RotateRightAsync()
        {
            return _scene3D.RotateXZAsync(2);
        }

        public Task RotateUpAsync()
        {
            return _scene3D.RotateYZAsync(2);
        }

        public Task RotateDownAsync()
        {
            return _scene3D.RotateYZAsync(-2);
        }

        public Task StopRotationAsync()
        {
            return _scene3D.StopRotationAsync();
        }

        public Task MoveCloser()
        {
            return _scene3D.MoveSceneCloser(0.1);
        }

        public Task MoveFurther()
        {
            return _scene3D.MoveSceneFurther(0.1);
        }

        private void CreateUmbrellaFigure()
        {
            // ЗОНТИК

            // шляпка
            var triangle1 = new Triangle3D(
                new Vector3D(-200, 0, 1000),
                new Vector3D(0, 200, 1000),
                new Vector3D(0, 0, 1200),
                Color.Blue);
            
            var triangle2 = new Triangle3D(
                new Vector3D(0, 200, 1000),
                new Vector3D(200, 0, 1000),
                new Vector3D(0, 0, 1200),
                Color.Red);
            
            var triangle3 = new Triangle3D(
                new Vector3D(200, 0, 1000),
                new Vector3D(0, -200, 1000),
                new Vector3D(0, 0, 1200),
                Color.Magenta);
            
            var triangle4 = new Triangle3D(
                new Vector3D(0, -200, 1000),
                new Vector3D(-200, 0, 1000),
                new Vector3D(0, 0, 1200),
                Color.Lime);
            
            // ножка
            
            Func<Vector3D, Vector3D, Vector3D, double> spinDeepPoint = (v1, v2, v3) =>
            {
                return Math.Sqrt(v3.X * v3.X + v3.Y * v3.Y + v3.Z * v3.Z);
            };
            
            var triangle5 = new Triangle3D(
                new Vector3D(-20 ,20, 800), 
                new Vector3D(20,20, 800),
                new Vector3D(0, 0, 1200),
                Color.Chocolate, spinDeepPoint);
            
            var triangle6 = new Triangle3D(
                new Vector3D(-20, -20, 800),
                new Vector3D(-20, 20, 800),
                new Vector3D(0, 0, 1200),
                Color.Chocolate, spinDeepPoint);
            
            var triangle7 = new Triangle3D(
                new Vector3D(20, -20, 800),
                new Vector3D(-20, -20, 800),
                new Vector3D(0, 0, 1200),
                Color.Chocolate, spinDeepPoint);

            var triangle8 = new Triangle3D(
                new Vector3D(20, 20, 800),
                new Vector3D(20, -20, 800),
                new Vector3D(0,0, 1200),
                Color.Chocolate, spinDeepPoint);
            
            var triangle9 = new Triangle3D(
                new Vector3D(-20, -20, 800), 
                new Vector3D(20, 20, 800),
                new Vector3D(-20, 20, 800),
                Color.Chocolate);
            
            var triangle10 = new Triangle3D(
                new Vector3D(20, 20, 800), 
                new Vector3D(-20, -20, 800), 
                new Vector3D(20, -20, 800), 
                Color.Chocolate);

            _scene3D.Append(triangle1);
            _scene3D.Append(triangle2);
            _scene3D.Append(triangle3);
            _scene3D.Append(triangle4);
            _scene3D.Append(triangle5);
            _scene3D.Append(triangle6);
            _scene3D.Append(triangle7);
            _scene3D.Append(triangle8);
            _scene3D.Append(triangle9);
            _scene3D.Append(triangle10);

            var averagePosition = Vector3D.Average(
                triangle1.OrientationVector,
                triangle2.OrientationVector,
                triangle3.OrientationVector,
                triangle4.OrientationVector,
                triangle5.OrientationVector,
                triangle6.OrientationVector,
                triangle7.OrientationVector,
                triangle8.OrientationVector,
                triangle9.OrientationVector,
                triangle10.OrientationVector);
            _scene3D.SetObjectPosition(averagePosition);
        }

        private void CreateSemiPetalUmbrellaFigure()
        {
            var petalsColors = new[]
            {
                Color.Blue,
                Color.Goldenrod,
                Color.Fuchsia,
                Color.Green,
                Color.Lime,
                Color.Indigo,
                Color.DarkOliveGreen,
            };
            
            var raduis = 200;
            var angle = 2 * Math.PI;
            var petalAngle = 2 * Math.PI / 7;
            for (var i = 0; i < 7; i++)
            {
                var vx = raduis * Math.Cos(angle);
                var vy = raduis * Math.Sin(angle);
                var v1 = new Vector3D((int)vx, (int)vy, 1100);
                angle -= petalAngle;
                vx = raduis * Math.Cos(angle);
                vy = raduis * Math.Sin(angle);
                var v2 = new Vector3D((int)vx, (int)vy, 1100);
                var v3 = new Vector3D(0,0, 1200);
                var triangle = new Triangle3D(v1, v2, v3, petalsColors[i]);
                _scene3D.Append(triangle);
            }
            
            // ножка
            
            Func<Vector3D, Vector3D, Vector3D, double> spinDeepPoint = (v1, v2, v3) =>
            {
                return Math.Sqrt(v3.X * v3.X + v3.Y * v3.Y + v3.Z * v3.Z);
            };
            
            var triangle5 = new Triangle3D(
                new Vector3D(-10 ,10, 800), 
                new Vector3D(10,10, 800),
                new Vector3D(0, 0, 1200),
                Color.Chocolate, spinDeepPoint);
            
            var triangle6 = new Triangle3D(
                new Vector3D(-10, -10, 800),
                new Vector3D(-10, 10, 800),
                new Vector3D(0, 0, 1200),
                Color.Chocolate, spinDeepPoint);
            
            var triangle7 = new Triangle3D(
                new Vector3D(10, -10, 800),
                new Vector3D(-10, -10, 800),
                new Vector3D(0, 0, 1200),
                Color.Chocolate, spinDeepPoint);

            var triangle8 = new Triangle3D(
                new Vector3D(10, 10, 800),
                new Vector3D(10, -10, 800),
                new Vector3D(0,0, 1200),
                Color.Chocolate, spinDeepPoint);
            
            var triangle9 = new Triangle3D(
                new Vector3D(-10, -10, 800), 
                new Vector3D(10, 10, 800),
                new Vector3D(-10, 10, 800),
                Color.SandyBrown);
            
            var triangle10 = new Triangle3D(
                new Vector3D(10, 10, 800), 
                new Vector3D(-10, -10, 800), 
                new Vector3D(10, -10, 800), 
                Color.SandyBrown);
            
            _scene3D.Append(triangle5);
            _scene3D.Append(triangle6);
            _scene3D.Append(triangle7);
            _scene3D.Append(triangle8);
            _scene3D.Append(triangle9);
            _scene3D.Append(triangle10);
            
            _scene3D.SetObjectPosition(new Vector3D(0,0, 1000));
        }
    }
}