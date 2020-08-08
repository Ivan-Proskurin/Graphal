using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        private List<Primitive2D> _primitives = new List<Primitive2D>();

        private ShiftTransform2D _shift = new ShiftTransform2D(0, 0);
        private Vector2D? _shiftStart;
        private readonly ConcurrentQueue<ShiftTransform2D> _transformsQueue = new ConcurrentQueue<ShiftTransform2D>();
        private CancellationTokenSource _cancellationTokenSource;

        public Scene2D(ICanvas2D canvas)
        {
            _canvas = canvas;
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

        public async Task RenderAsync()
        {
            _canvas.BeginDraw();

            try
            {
                _canvas.Clear();
                var renderingTasks = _primitives
                    .Select(x => Task.Run(() => x.Render(_canvas)));
                await Task.WhenAll(renderingTasks);
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
        }

        public event FpsChangedEventHandler FpsChanged;

        protected virtual void OnFpsChanged(FpsChangedArgs e)
        {
            FpsChanged?.Invoke(this, e);
        }

        private void ApplyTransform(Transform2D transform)
        {
            foreach (var primitive in _primitives)
            {
                primitive.Transform(transform);
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
                    var dequeued = _transformsQueue.TryDequeue(out var transform);
                    if (!dequeued)
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
                OnFpsChanged(new FpsChangedArgs(fps));
            }
        }
    }
}