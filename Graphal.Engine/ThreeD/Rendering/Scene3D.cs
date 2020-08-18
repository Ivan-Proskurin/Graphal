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
        private const int d = 300;

        private readonly IScene2D _scene2D;
        private readonly List<Triangle3D> _triangles = new List<Triangle3D>();
        private readonly ConcurrentQueue<RotationInfo> _rotationQueue = new ConcurrentQueue<RotationInfo>();
        private Task _rotationProcessingTask;
        private CancellationTokenSource _cancellationTokenSource;
        private ColorimetryInfo _colorimetry = new ColorimetryInfo
        {
            DiffusionRate = 0.6,
            LightFall = new Vector3D(12, 3, 5),
        };

        public Scene3D(IScene2D scene2D)
        {
            _scene2D = scene2D;
            _scene2D.SetShift(new Vector2D(700, 350));
        }

        public void Append(Triangle3D triangle)
        {
            _triangles.Add(triangle);
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

        public Task RotateXZAsync(int angles)
        {
            var rotationInfo = new RotationInfo
            {
                RotationType = RotationType.RotationXZ,
                RotateRadiansTotals = angles * Math.PI / 180,
            };
            _rotationQueue.Enqueue(rotationInfo);
            TryStartProcessRotationTask();
            return Task.CompletedTask;
        }

        public Task RotateYZAsync(int angles)
        {
            var rotationInfo = new RotationInfo
            {
                RotationType = RotationType.RotationYZ,
                RotateRadiansTotals = angles * Math.PI / 180,
            };
            _rotationQueue.Enqueue(rotationInfo);
            TryStartProcessRotationTask();
            return Task.CompletedTask;
        }

        public Task StopRotationAsync()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        public void SetObjectPosition(Vector3D v)
        {
            _triangles.ForEach(x => x.SetPosition(v));
        }

        public Task MoveSceneCloser(double grade)
        {
            _triangles.ForEach(x => x.MoveCloserByGrade(grade));
            ProjectSceneTo2D();
            return RenderAsync();
        }

        public Task MoveSceneFurther(double grade)
        {
            _triangles.ForEach(x => x.MoveFurtherByGrade(grade));
            ProjectSceneTo2D();
            return RenderAsync();
        }

        public event FpsChangedEventHandler FpsChanged;

        private void OnFpsChanged(FpsChangedArgs e)
        {
            FpsChanged?.Invoke(this, e);
        }

        private void ProjectSceneTo2D()
        {
            var sortedProjections = _triangles
                .OrderBy(x => x.NormalZ)
                .ThenBy(x => x.DeepLevel)
                .Select(x => x.Project(d, _colorimetry));
            
            _scene2D.FromProjection(sortedProjections);
        }

        private void TryStartProcessRotationTask()
        {
            if (_rotationProcessingTask != null)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _rotationProcessingTask = Task.Run(async () => await ProcessRotatesAsync());
        }

        private async Task ProcessRotatesAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var framesCount = 0;
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var rotationInfo = TryDequeueLastRotateInfo();
                    if (rotationInfo == null)
                    {
                        continue;
                    }

                    switch (rotationInfo.RotationType)
                    {
                        case RotationType.RotationXZ:
                            _triangles.ForEach(x => x.RotateXZ(rotationInfo.RotateRadiansTotals));
                            break;
                        case RotationType.RotationYZ:
                            _triangles.ForEach(x => x.RotateYZ(rotationInfo.RotateRadiansTotals));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    ProjectSceneTo2D();
                    await RenderAsync();
                    framesCount++;
                }

                _rotationProcessingTask = null;
            }
            finally
            {
                stopwatch.Stop();
                var fps = (int)(framesCount * 1000 / stopwatch.ElapsedMilliseconds);
                OnFpsChanged(new FpsChangedArgs(fps));
            }
        }

        private RotationInfo TryDequeueLastRotateInfo()
        {
            RotationInfo result = null;
            while (_rotationQueue.TryDequeue(out var info))
            {
                result = info;
            }

            return result;
        }
    }
}