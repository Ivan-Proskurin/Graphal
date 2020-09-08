using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Persistence.TwoD.Primitives;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Primitives
{
    public class Edge2D : Primitive2D
    {
        private readonly Vector2D _originalV1;
        private readonly Vector2D _originalV2;
        private Color _color;

        private Vector2D _v1;
        private Vector2D _v2;
        private Line2D _line;

        public Edge2D(Vector2D v1, Vector2D v2, Color color)
        {
            _originalV1 = v1;
            _originalV2 = v2;
            _v1 = v1;
            _v2 = v2;
            _color = color;
            UpdateGeometry();
        }

        public override Primitive2D Clone()
        {
            return new Edge2D(_originalV1, _originalV2, _color);
        }

        public override void Transform(Transform2D transform)
        {
            _v1 = transform.Apply(_originalV1);
            _v2 = transform.Apply(_originalV2);
            UpdateGeometry();
        }

        public override void Render(ICanvas2D canvas)
        {
            using (var enumerator = _line.Render(canvas, _color).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                }
            }
        }

        public override Primitive2Ds ToPrimitive2Ds()
        {
            return new Edge2Ds
            {
                V1 = _originalV1.ToVector2Ds(),
                V2 = _originalV2.ToVector2Ds(),
                Color = _color.ToArgb(),
            };
        }

        private void UpdateGeometry()
        {
            _line = new Line2D(_v1, _v2);
        }
    }
}