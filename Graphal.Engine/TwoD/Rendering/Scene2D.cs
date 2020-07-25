using System.Collections.Generic;
using System.Linq;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Persistence.TwoD;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Rendering
{
    public class Scene2D : IScene2D
    {
        private readonly ICanvas _canvas;
        private List<Primitive2D> _primitives = new List<Primitive2D>();

        private ShiftTransform2D _shift = new ShiftTransform2D(0, 0);
        private Vector2D? _shiftStart;

        public Scene2D(ICanvas canvas)
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

        public void Render()
        {
            _canvas.BeginDraw();

            try
            {
                _canvas.Clear();
                foreach (var primitive in _primitives)
                {
                    primitive.Render(_canvas);
                }
            }
            finally
            {
                _canvas.EndDraw();
            }
        }

        public void BeginShift(int x, int y)
        {
            _shiftStart = new Vector2D(x, y);
        }

        public void Shift(int x, int y)
        {
            var transform = GetShiftTransform(x, y);
            if (transform == null)
            {
                return;
            }

            transform.Combine(_shift);
            ApplyTransform(transform);

            Render();
        }

        public void EndShift(int x, int y)
        {
            var transform = GetShiftTransform(x, y);
            if (transform == null)
            {
                return;
            }

            Shift(x, y);
            _shift.Combine(transform);
            _shiftStart = null;
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

        private void ApplyTransform(Transform2D transform)
        {
            foreach (var primitive in _primitives)
            {
                primitive.Transform(transform);
            }
        }

        private ShiftTransform2D GetShiftTransform(int x, int y)
        {
            return _shiftStart == null
                ? null
                : new ShiftTransform2D(x - _shiftStart.Value.X, y - _shiftStart.Value.Y);
        }
    }
}