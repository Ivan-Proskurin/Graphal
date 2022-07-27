using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.ThreeD.Animation;
using Graphal.Engine.Abstractions.ThreeD.Rendering;
using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.ThreeD.Animation;
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
        private readonly IAnimationProcessor _animationProcessor;
        private readonly ILogger _logger;
        private readonly List<Primitive3D> _primitives = new List<Primitive3D>();
        private readonly List<Object3D> _objects = new List<Object3D>();
        private readonly Vector2D _visionShift = new Vector2D(700, 350);
        private Vector2DR _startRotatePoint;
        private readonly ColorimetryInfo _colorimetry = new ColorimetryInfo
        {
            DiffusionRate = 0.6,
            LightFall = new Vector3D(12, 3, 5),
        };

        public Scene3D(IScene2D scene2D, IAnimationProcessor animationProcessor, ILogger logger)
        {
            _scene2D = scene2D;
            _animationProcessor = animationProcessor;
            _logger = logger;
            _animationProcessor.ProcessAnimation += AnimationProcessorOnProcessAnimation;
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

        public void Append(Object3D object3D)
        {
            _objects.Add(object3D);
            _primitives.AddRange(object3D.GetTriangles());
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
            _animationProcessor.EnqueueAnimation(new MoveCloserAnimation(grade));
            return Task.CompletedTask;
        }

        public Task MoveSceneFurther(double grade)
        {
            _animationProcessor.EnqueueAnimation(new MoveFurtherAnimation(grade));
            return Task.CompletedTask;
        }

        public Task StartRotateAsync(double x, double y)
        {
            _startRotatePoint = new Vector2DR(x, y);
            var rotationInfo = new ModelRotationAnimation(RotationPhase.Start, 0, 0);
            _animationProcessor.EnqueueAnimation(rotationInfo);
            return Task.CompletedTask;
        }

        public Task ContinueRotateAsync(double x, double y)
        {
            var rotationInfo = GetRotationInfo(RotationPhase.Rotate, x, y);
            if (rotationInfo == null)
            {
                return null;
            }
            _animationProcessor.EnqueueAnimation(rotationInfo);
            return Task.CompletedTask;
        }

        public Task StopRotateAsync(double x, double y)
        {
            _startRotatePoint = null;
            var rotationInfo = new ModelRotationAnimation(RotationPhase.Stop, 0, 0);
            _animationProcessor.EnqueueAnimation(rotationInfo);
            return Task.CompletedTask;
        }

        public Task RotateCubeDimension(bool reverse)
        {
            _objects.ForEach(x => x.RotateCubeDimension(reverse));
            return RenderAsync();
        }

        public Task RotateCubeDimension(int cubeDimension, bool reverse)
        {
            _objects.ForEach(x => x.RotateCubeDimension(cubeDimension, reverse));
            return RenderAsync();
        }

        private ModelRotationAnimation GetRotationInfo(RotationPhase phase, double x, double y)
        {
            if (_startRotatePoint == null)
            {
                return null;
            }

            return new ModelRotationAnimation(phase,
                GetRotationRadians(_startRotatePoint.X, x),
                GetRotationRadians(_startRotatePoint.Y, y)
            );
        }

        private static double GetRotationRadians(double start, double end)
        {
            const int fullRotateLength = 500;
            return 2 * Math.PI * (end - start) / fullRotateLength;
        }

        private void ProjectSceneTo2D()
        {
            var sortedTriangles = _primitives
                .Where(x => x.CalculateNormalZ() > 0)
                .OrderByDescending(x => x.DeepLevel())
                .ToArray();
            
            var projections = sortedTriangles.Select(x => x.Project(D, _colorimetry, x));
            _scene2D.FromProjection(projections);
        }
        
        private async Task AnimationProcessorOnProcessAnimation(ProcessAnimationEventArgs e)
        {
            if (e.AnimationInfo is ModelRotationAnimation rotation)
            {
                switch (rotation.Phase)
                {
                    case RotationPhase.Start:
                        _primitives.ForEach(x => x.StartRotation());
                        _objects.ForEach(x => x.StartRotation());
                        break;
                    case RotationPhase.Rotate:
                        _primitives.ForEach(x => x.Rotate(rotation.RadiansXZ, rotation.RadiansYZ));
                        _objects.ForEach(x => x.Rotate(rotation.RadiansXZ, rotation.RadiansYZ));
                        break;
                    case RotationPhase.Stop:
                        _primitives.ForEach(x => x.StopRotation());
                        _objects.ForEach(x => x.StopRotation());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                await RenderAsync();
            }

            if (e.AnimationInfo is MoveCloserAnimation moveCloser)
            {
                _primitives.ForEach(x => x.MoveCloserByGrade(moveCloser.Grade));
                _objects.ForEach(x => x.MoveCloserByGrade(moveCloser.Grade));
                await RenderAsync();
            }

            if (e.AnimationInfo is MoveFurtherAnimation moveFurther)
            {
                _primitives.ForEach(x => x.MoveFurtherByGrade(moveFurther.Grade));
                _objects.ForEach(x => x.MoveFurtherByGrade(moveFurther.Grade));
                await RenderAsync();
            }
        }
    }
}