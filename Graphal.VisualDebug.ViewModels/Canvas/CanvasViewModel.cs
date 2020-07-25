﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Profile;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;
using Graphal.Tools.Abstractions.Persistence;
using Graphal.VisualDebug.Abstractions.Canvas;

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
        
        private readonly IScenePersistenceService _scenePersistenceService;
        private readonly IPerformanceProfiler _performanceProfiler;
        private readonly IScene2D _scene;
        private readonly IBitmapCanvas _canvas;
        private readonly List<Vector2D> _vectors = new List<Vector2D>();
        private int _framesCounter;
        private int _colorIndex;
        private Stopwatch _stopwatch;

        public CanvasViewModel(
            IScenePersistenceService scenePersistenceService,
            IPerformanceProfiler performanceProfiler,
            IScene2D scene,
            IBitmapCanvas canvas)
        {
            _scenePersistenceService = scenePersistenceService;
            _performanceProfiler = performanceProfiler;
            _scene = scene;
            _canvas = canvas;
        }

        public object ImageSource => _canvas.Bitmap;

        public async Task InitializeAsync()
        {
            using (var session = _performanceProfiler.CreateSession())
            {
                var sceneContainer = await _scenePersistenceService.LoadScene2DAsync();
                if (sceneContainer == null)
                {
                    var triangle = new Triangle2D(
                        new Vector2D(-50, -50),
                        new Vector2D(0, 50),
                        new Vector2D(50, -50),
                        GetNextColor());
                    
                    _scene.Append(triangle);
                    _scene.BeginShift(0, 0);
                    _scene.EndShift(_canvas.Width / 2, _canvas.Height / 2);
                    return;
                }

                _scene.FromScene2Ds(sceneContainer);
                _scene.Render();
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
            _canvas.SetSize(width, height);
        }

        public void BeginShift(int x, int y)
        {
            _scene.BeginShift(x, y);
            _framesCounter = 0;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Shift(int x, int y)
        {
            _scene.Shift(x, y);
            _framesCounter++;
        }

        public void EndShift(int x, int y)
        {
            using (var session = _performanceProfiler.CreateSession())
            {
                _scene.EndShift(x, y);
                _stopwatch.Stop();
                session.LogWithPerformance($"FPS = {_framesCounter * 1000 / _stopwatch.ElapsedMilliseconds}");
                _framesCounter = 0;
            }
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
    }
}