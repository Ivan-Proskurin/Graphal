using System;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.ThreeD.Rendering;
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

        public CanvasViewModel3d(
            IBitmapSource bitmapSource,
            IScene3D scene3D,
            ILogger logger)
        {
            _bitmapSource = bitmapSource;
            _scene3D = scene3D;
            _logger = logger;
        }

        public object ImageSource => _bitmapSource.Bitmap;

        public void Resize(int width, int height)
        {
        }

        public async Task InitializeAsync()
        {
            await _scene3D.InitializeAsync();
            // CreateRubiksColorFacet();
            // CreateRubiksDices();
            await CreateRubikCube();

            await _scene3D.RenderAsync();
        }

        public Task MoveCloser(double grade)
        {
            return _scene3D.MoveSceneCloser(grade);
        }

        public Task MoveFurther(double grade)
        {
            return _scene3D.MoveSceneFurther(grade);
        }

        public Task MoveCloser()
        {
            return _scene3D.MoveSceneCloser(0.1);
        }

        public Task MoveFurther()
        {
            return _scene3D.MoveSceneFurther(0.1);
        }

        public Task StartRotateAsync(double x, double y)
        {
            return _scene3D.StartRotateAsync(x, y);
        }

        public Task ContinueRotateAsync(double x, double y)
        {
            return _scene3D.ContinueRotateAsync(x, y);
        }

        public Task StopRotateAsync(double x, double y)
        {
            return _scene3D.StopRotateAsync(x, y);
        }

        public Task RotateCubeDimension(bool reverse)
        {
            return _scene3D.RotateCubeDimension(reverse);
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
            
            var dice1 = new RubiksDice(100, position1, new[]{CubeDimension.South, CubeDimension.Top, CubeDimension.West});
            var dice2 = new RubiksDice(100, position2, Array.Empty<CubeDimension>());

            _scene3D.Append(dice1.GetTriangles());
            _scene3D.Append(dice2.GetTriangles());
            
            _scene3D.SetObjectPosition(position);
        }

        private async Task CreateRubikCube()
        {
            var position = new Vector3D(0, 0, 1000);
            var cube = new RubikCube(position);
            _scene3D.Append(cube);
            _scene3D.SetObjectPosition(position);
            await _scene3D.RotateCubeDimension((int)CubeDimension.East, false);
            await _scene3D.RotateCubeDimension((int)CubeDimension.West, false);
            await _scene3D.RotateCubeDimension((int)CubeDimension.Top, true);
            await _scene3D.RotateCubeDimension((int)CubeDimension.MiddleWestEast, false);
            await _scene3D.RotateCubeDimension((int)CubeDimension.MiddleSouthNorth, false);
            await _scene3D.RotateCubeDimension((int)CubeDimension.North, false);
            await _scene3D.RotateCubeDimension((int)CubeDimension.East, true);
            await _scene3D.RotateCubeDimension((int)CubeDimension.Bottom, false);
            await _scene3D.RotateCubeDimension((int)CubeDimension.South, true);
            await _scene3D.RotateCubeDimension((int)CubeDimension.MiddleTopBottom, false);
        }
    }
}