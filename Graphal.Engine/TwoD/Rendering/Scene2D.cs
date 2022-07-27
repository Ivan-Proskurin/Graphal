using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.IntersectBehaviours;
using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Persistence.TwoD;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Rendering
{
    public class Scene2D : IScene2D
    {
        private readonly ICanvas2D _canvas;
        private readonly IIntersectionFactory _intersectionFactory;
        private readonly ILogger _logger;
        private List<Primitive2D> _primitives = new List<Primitive2D>();
        private List<RenderingFrame> _frames = new List<RenderingFrame>();

        private ShiftTransform2D _shift = new ShiftTransform2D(0, 0);
        private Vector2D? _shiftStart;
        private readonly ConcurrentQueue<ShiftTransform2D> _transformsQueue = new ConcurrentQueue<ShiftTransform2D>();
        private CancellationTokenSource _cancellationTokenSource;

        public Scene2D(ICanvas2D canvas, IIntersectionFactory intersectionFactory, ILogger logger)
        {
            _canvas = canvas;
            _intersectionFactory = intersectionFactory;
            _logger = logger;
        }

        public void Append(Primitive2D primitive)
        {
            primitive.Transform(_shift);
            _primitives.Add(primitive);

            _canvas.BeginDraw();
            try
            {
                primitive.Render(_canvas);
            }
            finally
            {
                _canvas.EndDraw();
            }
        }

        public void SetShift(Vector2D shift)
        {
            _shift = new ShiftTransform2D(shift.X, shift.Y);
        }

        public async Task RenderAsync()
        {
            _canvas.BeginDraw();

            try
            {
                _canvas.Clear();
                foreach (var primitive in _primitives)
                {
                    primitive.Render(_canvas);
                }
                
                await Task.CompletedTask;
                // var renderingTasks = _frames
                //     .Select(x => Task.Run(() => x.Render(_canvas)))
                //     .ToArray();
                // await Task.WhenAll(renderingTasks);
            }
            finally
            {
                _canvas.EndDraw();
            }
        }

        public async Task BeginShiftAsync(int x, int y)
        {
            _shiftStart = new Vector2D(x, y);
            _cancellationTokenSource = new CancellationTokenSource();
            await Task.Run(async () => await ProcessTransformsAsync());
        }

        public Task<ShiftTransform2D> ShiftAsync(int x, int y)
        {
            var transform = GetNextShiftTransform(x, y);
            _transformsQueue.Enqueue(transform);
            return Task.FromResult(transform);
        }

        public async Task EndShiftAsync(int x, int y)
        {
            var transform = await ShiftAsync(x, y);
            _shift = transform;
            _shiftStart = null;
            _cancellationTokenSource.Cancel();
        }

        public Vector2D ToWorldCoordinates(Vector2D v)
        {
            return _shift.Invert(v);
        }

        public Scene2Ds ToScene2Ds()
        {
            return new Scene2Ds
            {
                Transform = _shift.ToTransform2Ds(),
                Primitives = _primitives.Select(x => x.ToPrimitive2Ds()).ToList(),
            };
        }

        public void FromScene2Ds(Scene2Ds container)
        {
            if (container.Transform.ToTransform2D() is ShiftTransform2D shiftTransform)
            {
                _shift = shiftTransform;
            }

            _primitives = container.Primitives.Select(x => x.ToPrimitive2D()).ToList();
            ApplyTransform(_shift);
            CreateRenderingFrames();
        }

        public void FromProjection(IEnumerable<Primitive2D> primitives)
        {
            _primitives = primitives.ToList();
            ApplyTransform(_shift);
        }

        private void ApplyTransform(Transform2D transform)
        {
            foreach (var primitive in _primitives)
            {
                primitive.Transform(transform);
            }
        }

        private void CreateRenderingFrames()
        {
            _frames.Clear();
            
            var primitives = _primitives.Select(x => x).ToList();
            if (!primitives.Any())
            {
                return;
            }

            while (primitives.Count > 0)
            {
                var target = primitives.First();
                primitives.Remove(target);
                var pairs = primitives.Skip(1).ToList();
                var together = new List<Primitive2D>();
                together.Add(target);
                foreach (var pair in pairs)
                {
                    var intersection = _intersectionFactory.CreateBehaviour(target, pair);
                    if (intersection.Intersects())
                    {
                        together.Add(pair);
                        primitives.Remove(pair);
                    }
                }

                var frame = new RenderingFrame(together, _intersectionFactory);
                _frames.Add(frame);
            }

            if (_frames.Sum(x => x.Capacity) != _primitives.Count)
            {
                throw new InvalidConstraintException();
            }
        }

        private ShiftTransform2D GetNextShiftTransform(int x, int y)
        {
            var transform = _shiftStart == null
                ? null
                : new ShiftTransform2D(x - _shiftStart.Value.X, y - _shiftStart.Value.Y);

            if (transform == null)
            {
                throw new InvalidOperationException("Shift is not started");
            }
            
            transform.Combine(_shift);
            return transform;
        }

        private async Task ProcessTransformsAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var framesCount = 0;
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var transform = DequeueLastTransform();
                    if (transform == null)
                    {
                        continue;
                    }

                    ApplyTransform(transform);
                    await RenderAsync();
                    framesCount++;
                }
            }
            finally
            {
                stopwatch.Stop();
                var fps = (int)(framesCount * 1000 / stopwatch.ElapsedMilliseconds);
                _logger.Info($"FPS: {fps}");
            }
        }

        private ShiftTransform2D DequeueLastTransform()
        {
            ShiftTransform2D result = null;
            while (_transformsQueue.TryDequeue(out var transform))
            {
                result = transform;
            }

            return result;
        }
    }
}