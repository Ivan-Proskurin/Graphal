using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.Profile;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;
using Graphal.Tools.Abstractions.Persistence;
using Graphal.VisualDebug.Abstractions.Canvas;
using Graphal.VisualDebug.Abstractions.Wrappers;

namespace Graphal.VisualDebug.ViewModels.Canvas
{
    public class CanvasViewModel : ICanvasViewModel
    {
        private static readonly Color[] Colors =
        {
            Color.Aqua,
            Color.LimeGreen,
            Color.Goldenrod,
            Color.Brown,
            Color.DarkOrchid,
        };

        private readonly ILogger _logger;
        private readonly IScenePersistenceService _scenePersistenceService;
        private readonly IPerformanceProfiler _performanceProfiler;
        private readonly IScene2D _scene;
        private readonly IBitmapSource _bitmapSource;
        private readonly IDispatcherWrapper _dispatcherWrapper;
        private readonly List<Vector2D> _vectors = new List<Vector2D>();
        private int _colorIndex;

        private int _width;
        private int _height;

        public CanvasViewModel(
            ILogger logger,
            IScenePersistenceService scenePersistenceService,
            IPerformanceProfiler performanceProfiler,
            IScene2D scene,
            IBitmapSource bitmapSource,
            IDispatcherWrapper dispatcherWrapper)
        {
            _logger = logger;
            _scenePersistenceService = scenePersistenceService;
            _performanceProfiler = performanceProfiler;
            _scene = scene;
            _bitmapSource = bitmapSource;
            _dispatcherWrapper = dispatcherWrapper;
        }

        public object ImageSource => _bitmapSource.Bitmap;

        public async Task InitializeAsync()
        {
            using (var session = _performanceProfiler.CreateSession())
            {
                _scene.FpsChanged += SceneOnFpsChanged;
                var sceneContainer = await _scenePersistenceService.LoadScene2DAsync();
                if (sceneContainer == null)
                {
                    var triangle = new Triangle2D(
                        new Vector2D(-50, -50),
                        new Vector2D(0, 50),
                        new Vector2D(50, -50),
                        GetNextColor());
                    
                    _scene.Append(triangle);
                    await _scene.BeginShiftAsync(0, 0);
                    await _scene.EndShiftAsync(_width / 2, _height / 2);
                    return;
                }

                _scene.FromScene2Ds(sceneContainer);
                await _scene.RenderAsync();
                session.LogWithPerformance("2D scene loaded");
            }
        }

        public Task StoreSceneAsync()
        {
            var sceneContainer = _scene.ToScene2Ds();
            return _scenePersistenceService.SaveScene2DAsync(sceneContainer);
        }

        public void SetPoint(int x, int y)
        {
            var v = _scene.ToWorldCoordinates(new Vector2D(x, y));

            var point = new Point2D(v, Color.Aqua);
            _vectors.Add(v);
            using (var session = _performanceProfiler.CreateSession())
            {
                _scene.Append(point);
                session.LogWithPerformance($"Set point at ({x}; {y})");
            }

            if (_vectors.Count != 3) return;
            
            var triangle = new Triangle2D(_vectors[0], _vectors[1], _vectors[2], GetNextColor());
            using (var session = _performanceProfiler.CreateSession())
            {
                _scene.Append(triangle);
                session.LogWithPerformance("Draw triangle");
            }
            
            _vectors.Clear();

            // if (_vectors.Count != 2) return;
            //
            // var edge = new Edge2D(_vectors[0], _vectors[1], Color.Aqua);
            // using (var session = _performanceProfiler.CreateSession())
            // {
            //     _scene.Append(edge);
            //     session.LogWithPerformance("Draw edge");
            // }
            //
            // _vectors.Clear();
        }

        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public Task BeginShiftAsync(int x, int y)
        {
            return _scene.BeginShiftAsync(x, y);
        }

        public Task ShiftAsync(int x, int y)
        {
            return _scene.ShiftAsync(x, y);
        }

        public Task EndShiftAsync(int x, int y)
        {
            return _scene.EndShiftAsync(x, y);
        }

        private Color GetNextColor()
        {
            var color = Colors[_colorIndex++];
            if (_colorIndex >= Colors.Length)
            {
                _colorIndex = 0;
            }

            return color;
        }

        private void SceneOnFpsChanged(object sender, FpsChangedArgs e)
        {
            _dispatcherWrapper.Invoke(() => _logger.Info($"FPS: {e.Fps}"));
        }
    }
}