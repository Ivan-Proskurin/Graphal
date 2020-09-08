using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.ThreeD.Rendering;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;
using Graphal.Engine.TwoD.Geometry;

namespace Graphal.Engine.ThreeD.Rendering
{
    public class Scene3D : IScene3D
    {
        private const int D = 300;

        private readonly IScene2D _scene2D;
        private readonly List<Primitive3D> _primitives = new List<Primitive3D>();
        private readonly ConcurrentQueue<RotationInfo> _rotationQueueXZ = new ConcurrentQueue<RotationInfo>();
        private readonly ConcurrentQueue<RotationInfo> _rotationQueueYZ = new ConcurrentQueue<RotationInfo>();
        private Task _rotationProcessingTask;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Vector2D _visionShift = new Vector2D(700, 350);
        private Vector2D? _startRotatePoint;
        private int _rotateFps;
        private readonly ColorimetryInfo _colorimetry = new ColorimetryInfo
        {
            DiffusionRate = 0.6,
            LightFall = new Vector3D(12, 3, 5),
        };

        public Scene3D(IScene2D scene2D)
        {
            _scene2D = scene2D;
            _scene2D.SetShift(_visionShift);
        }

        public void Append(Triangle3D triangle)
        {
            _primitives.Add(triangle);
        }

        public void Append(IEnumerable<Triangle3D> triangles)
        {
            _primitives.AddRange(triangles);
        }

        public void Append(Edge3D edge)
        {
            _primitives.Add(edge);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task RenderAsync()
        {
            ProjectSceneTo2D();
            return _scene2D.RenderAsync();
        }

        public void SetObjectPosition(Vector3D v)
        {
            _primitives.ForEach(x => x.SetPosition(v));
        }

        public Task MoveSceneCloser(double grade)
        {
            _primitives.ForEach(x => x.MoveCloserByGrade(grade));
            ProjectSceneTo2D();
            return RenderAsync();
        }

        public Task MoveSceneFurther(double grade)
        {
            _primitives.ForEach(x => x.MoveFurtherByGrade(grade));
            ProjectSceneTo2D();
            return RenderAsync();
        }

        public Task StartRotateAsync(int x, int y)
        {
            _startRotatePoint = new Vector2D(x, y);
            return Task.CompletedTask;
        }

        public Task ContinueRotateAsync(int x, int y)
        {
            foreach (var rotationInfo in GetRotationInfos(x, y, false))
            {
                switch (rotationInfo.RotationType)
                {
                    case RotationType.RotationXZ:
                    case RotationType.StopRotationXZ:
                        _rotationQueueXZ.Enqueue(rotationInfo);
                        break;
                    case RotationType.RotationYZ:
                    case RotationType.StopRotationYZ:
                        _rotationQueueYZ.Enqueue(rotationInfo);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            TryStartProcessRotationTask();
            return Task.CompletedTask;
        }

        public async Task StopRotateAsync(int x, int y)
        {
            foreach (var rotationInfo in GetRotationInfos(x, y, true))
            {
                switch (rotationInfo.RotationType)
                {
                    case RotationType.RotationXZ:
                    case RotationType.StopRotationXZ:
                        _rotationQueueXZ.Enqueue(rotationInfo);
                        break;
                    case RotationType.RotationYZ:
                    case RotationType.StopRotationYZ:
                        _rotationQueueYZ.Enqueue(rotationInfo);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _cancellationTokenSource.Cancel();
            if (_rotationProcessingTask != null)
            {
                await _rotationProcessingTask;
            }

            _rotationProcessingTask = null;
            _startRotatePoint = null;
            OnFpsChanged(new FpsChangedArgs(_rotateFps));
        }

        public event FpsChangedEventHandler FpsChanged;

        private void OnFpsChanged(FpsChangedArgs e)
        {
            FpsChanged?.Invoke(this, e);
        }

        private IEnumerable<RotationInfo> GetRotationInfos(int x, int y, bool stop)
        {
            if (_startRotatePoint == null)
            {
                yield break;
            }

            yield return new RotationInfo
            {
                RotationType = stop ? RotationType.StopRotationXZ : RotationType.RotationXZ,
                RotateRadiansTotals = GetRotationRadians(_startRotatePoint.Value.X, x),
            };

            if (_startRotatePoint == null)
            {
                yield break;
            }
            
            yield return new RotationInfo
            {
                RotationType = stop ? RotationType.StopRotationYZ : RotationType.RotationYZ,
                RotateRadiansTotals = GetRotationRadians(_startRotatePoint.Value.Y, y),
            };
        }

        private static double GetRotationRadians(int start, int end)
        {
            const int fullRotateLength = 500;
            return 2 * Math.PI * (end - start) / fullRotateLength;
        }

        private void ProjectSceneTo2D()
        {
            var sortedTriangles = _primitives
                .Where(x => x.NormalZ > 0)
                .OrderByDescending(x => x.DeepLevel())
                .ToArray();
            
            var projections = sortedTriangles.Select(x => x.Project(D, _colorimetry));
            _scene2D.FromProjection(projections);
        }

        private void TryStartProcessRotationTask()
        {
            if (_rotationProcessingTask != null)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _rotationProcessingTask = Task.Run(async () => await ProcessRotationAsync());
        }

        private async Task ProcessRotationAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var framesCount = 0;
            try
            {
                while (true)
                {
                    var (rotationXz, rotationYz) = TryDequeueLastRotateInfo();
                    if (rotationXz == null && rotationYz == null)
                    {
                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            break;
                        }
                        continue;
                    }

                    if (rotationXz != null)
                    {
                        switch (rotationXz.RotationType)
                        {
                            case RotationType.RotationXZ:
                                _primitives.ForEach(x => x.RotateXZ(rotationXz.RotateRadiansTotals));
                                break;
                            case RotationType.StopRotationXZ:
                                _primitives.ForEach(x => x.ApplyRotationXZ(rotationXz.RotateRadiansTotals));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    if (rotationYz != null)
                    {
                        switch (rotationYz.RotationType)
                        {
                            case RotationType.RotationYZ:
                                _primitives.ForEach(x => x.RotateYZ(rotationYz.RotateRadiansTotals));
                                break;
                            case RotationType.StopRotationYZ:
                                _primitives.ForEach(x => x.ApplyRotationYZ(rotationYz.RotateRadiansTotals));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    ProjectSceneTo2D();
                    await RenderAsync();
                    framesCount++;
                }
            }
            finally
            {
                stopwatch.Stop();
                _rotateFps = (int)(framesCount * 1000 / stopwatch.ElapsedMilliseconds);
            }
        }

        private (RotationInfo xz, RotationInfo yz) TryDequeueLastRotateInfo()
        {
            RotationInfo resultXz = null;
            while (_rotationQueueXZ.TryDequeue(out var info))
            {
                resultXz = info;
            }

            RotationInfo resultYz = null;
            while (_rotationQueueYZ.TryDequeue(out var info))
            {
                resultYz = info;
            }

            return (resultXz, resultYz);
        }
    }
}