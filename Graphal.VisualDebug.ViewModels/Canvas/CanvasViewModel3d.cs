using System;
using System.Drawing;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.ThreeD.Rendering;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.RubiksCube.Core;
using Graphal.RubiksCube.Core.Dices;
using Graphal.VisualDebug.Abstractions.Canvas;

namespace Graphal.VisualDebug.ViewModels.Canvas
{
    public class CanvasViewModel3d : ICanvasViewModel3d
    {
        private readonly IBitmapSource _bitmapSource;
        private readonly IScene3D _scene3D;
        private readonly ILogger _logger;
        private readonly IScene2D _scene2D;

        public CanvasViewModel3d(
            IBitmapSource bitmapSource,
            IScene3D scene3D,
            ILogger logger,
            IScene2D scene2D)
        {
            _bitmapSource = bitmapSource;
            _scene3D = scene3D;
            _logger = logger;
            _scene2D = scene2D;
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
            // CreateSemiPetalUmbrellaFigure();
            // CreateRubiksColorFacet();
            // CreateRubiksDices();
            CreateRubikCube();
            await _scene3D.RenderAsync();
        }

        private void Scene3DOnFpsChanged(object sender, FpsChangedArgs e)
        {
            _logger.Info($"FPS: {e.Fps}");
        }

        public Task MoveCloser()
        {
            return _scene3D.MoveSceneCloser(0.1);
        }

        public Task MoveFurther()
        {
            return _scene3D.MoveSceneFurther(0.1);
        }

        public Task StartRotateAsync(int x, int y)
        {
            return _scene3D.StartRotateAsync(x, y);
        }

        public Task ContinueRotateAsync(int x, int y)
        {
            return _scene3D.ContinueRotateAsync(x, y);
        }

        public Task StopRotateAsync(int x, int y)
        {
            return _scene3D.StopRotateAsync(x, y);
        }

        private void CreateUmbrellaFigure()
        {
            // ЗОНТИК

            // шляпка
            // var triangle1 = new Triangle3D(
            //     new Vector3D(-200, 0, 1000),
            //     new Vector3D(0, 200, 1000),
            //     new Vector3D(0, 0, 1200),
            //     Color.Blue);
            //
            // var triangle2 = new Triangle3D(
            //     new Vector3D(0, 200, 1000),
            //     new Vector3D(200, 0, 1000),
            //     new Vector3D(0, 0, 1200),
            //     Color.Red);
            //
            // var triangle3 = new Triangle3D(
            //     new Vector3D(200, 0, 1000),
            //     new Vector3D(0, -200, 1000),
            //     new Vector3D(0, 0, 1200),
            //     Color.Magenta);
            //
            // var triangle4 = new Triangle3D(
            //     new Vector3D(0, -200, 1000),
            //     new Vector3D(-200, 0, 1000),
            //     new Vector3D(0, 0, 1200),
            //     Color.Lime);
            
            // ножка
            
            Func<Vector3D, Vector3D, Vector3D, double> spinDeepPoint = (v1, v2, v3) =>
            {
                return Math.Sqrt(v3.X * v3.X + v3.Y * v3.Y + v3.Z * v3.Z);
            };
            
            // var triangle5 = new Triangle3D(
            //     new Vector3D(-20 ,20, 800), 
            //     new Vector3D(20,20, 800),
            //     new Vector3D(0, 0, 1200),
            //     Color.Chocolate);
            //
            // var triangle6 = new Triangle3D(
            //     new Vector3D(-20, -20, 800),
            //     new Vector3D(-20, 20, 800),
            //     new Vector3D(0, 0, 1200),
            //     Color.Chocolate);
            //
            // var triangle7 = new Triangle3D(
            //     new Vector3D(20, -20, 800),
            //     new Vector3D(-20, -20, 800),
            //     new Vector3D(0, 0, 1200),
            //     Color.Chocolate);
            //
            // var triangle8 = new Triangle3D(
            //     new Vector3D(20, 20, 800),
            //     new Vector3D(20, -20, 800),
            //     new Vector3D(0,0, 1200),
            //     Color.Chocolate);
            //
            // var triangle9 = new Triangle3D(
            //     new Vector3D(-20, -20, 800), 
            //     new Vector3D(20, 20, 800),
            //     new Vector3D(-20, 20, 800),
            //     Color.Chocolate);
            //
            // var triangle10 = new Triangle3D(
            //     new Vector3D(20, 20, 800), 
            //     new Vector3D(-20, -20, 800), 
            //     new Vector3D(20, -20, 800), 
            //     Color.Chocolate);
            //
            // _scene3D.Append(triangle1);
            // _scene3D.Append(triangle2);
            // _scene3D.Append(triangle3);
            // _scene3D.Append(triangle4);
            // _scene3D.Append(triangle5);
            // _scene3D.Append(triangle6);
            // _scene3D.Append(triangle7);
            // _scene3D.Append(triangle8);
            // _scene3D.Append(triangle9);
            // _scene3D.Append(triangle10);

            // var averagePosition = Vector3D.Average(
            //     triangle1.OrientationVector,
            //     triangle2.OrientationVector,
            //     triangle3.OrientationVector,
            //     triangle4.OrientationVector,
            //     triangle5.OrientationVector,
            //     triangle6.OrientationVector,
            //     triangle7.OrientationVector,
            //     triangle8.OrientationVector,
            //     triangle9.OrientationVector,
            //     triangle10.OrientationVector);
            // _scene3D.SetObjectPosition(averagePosition);
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
                // var triangle = new Triangle3D(v1, v2, v3, petalsColors[i]);
                // _scene3D.Append(triangle);
            }
            
            // ножка
            
            Func<Vector3D, Vector3D, Vector3D, double> spinDeepPoint = (v1, v2, v3) =>
            {
                return Math.Sqrt(v3.X * v3.X + v3.Y * v3.Y + v3.Z * v3.Z);
            };
            
            // var triangle5 = new Triangle3D(
            //     new Vector3D(-10 ,10, 800), 
            //     new Vector3D(10,10, 800),
            //     new Vector3D(0, 0, 1200),
            //     Color.Chocolate);
            //
            // var triangle6 = new Triangle3D(
            //     new Vector3D(-10, -10, 800),
            //     new Vector3D(-10, 10, 800),
            //     new Vector3D(0, 0, 1200),
            //     Color.Chocolate);
            //
            // var triangle7 = new Triangle3D(
            //     new Vector3D(10, -10, 800),
            //     new Vector3D(-10, -10, 800),
            //     new Vector3D(0, 0, 1200),
            //     Color.Chocolate);
            //
            // var triangle8 = new Triangle3D(
            //     new Vector3D(10, 10, 800),
            //     new Vector3D(10, -10, 800),
            //     new Vector3D(0,0, 1200),
            //     Color.Chocolate);
            //
            // var triangle9 = new Triangle3D(
            //     new Vector3D(-10, -10, 800), 
            //     new Vector3D(10, 10, 800),
            //     new Vector3D(-10, 10, 800),
            //     Color.SandyBrown);
            //
            // var triangle10 = new Triangle3D(
            //     new Vector3D(10, 10, 800), 
            //     new Vector3D(-10, -10, 800), 
            //     new Vector3D(10, -10, 800), 
            //     Color.SandyBrown);
            //
            // _scene3D.Append(triangle5);
            // _scene3D.Append(triangle6);
            // _scene3D.Append(triangle7);
            // _scene3D.Append(triangle8);
            // _scene3D.Append(triangle9);
            // _scene3D.Append(triangle10);
            //
            _scene3D.SetObjectPosition(new Vector3D(0,0, 1000));
        }

        private void CreateRubiksColorFacet()
        {
            var position1 = new Vector3D(0,0, 1000);
            var facet1 = new CubeColorFacet(100, position1, FacetDimensions.XY, null, false);
            // var position2 = new Vector3D(50,0, 1000);
            // var facet2 = new CubeColorFacet(100, position2, FacetOrientation.East, null);
            _scene3D.Append(facet1);
            // _scene3D.Append(facet2);
            _scene3D.SetObjectPosition(new Vector3D(0, 0,  800));
            // _scene3D.Append(new Edge3D(
            //     new Vector3D(0, 750, 800),
            //     new Vector3D(0, 850, 800),
            //     Color.Aqua));
        }

        private void CreateRubiksDices()
        {
            var position = new Vector3D(0, 0, 1000);
            var position1 = new Vector3D(0, 0, 948);
            var position2 = new Vector3D(0,0, 1052);
            
            var dice1 = new RubiksDice(100, position1);
            dice1.AddFacet(FacetOrientation.South, Color.Beige);
            dice1.AddFacet(FacetOrientation.Top, Color.Red);
            dice1.AddFacet(FacetOrientation.West, Color.Blue);
            // dice1.AddFacet(FacetOrientation.East, Color.Lime);
            // dice1.AddFacet(FacetOrientation.Bottom, Color.Aqua);
            // dice1.AddFacet(FacetOrientation.North, Color.Maroon);
            // dice1.AddFacet(FacetOrientation.West, Color.Blue);
            
            var dice2 = new RubiksDice(100, position2);
            
            _scene3D.Append(dice1.GetTriangles());
            _scene3D.Append(dice2.GetTriangles());
            
            _scene3D.SetObjectPosition(position);
        }

        private void CreateRubikCube()
        {
            var position = new Vector3D(0, 0, 1000);
            var cube = new RubikCube(position);
            _scene3D.Append(cube.GetTriangles());
            _scene3D.SetObjectPosition(position);
        }
    }
}