using System.Drawing;

using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.Persistence.TwoD.Primitives;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.TwoD.Primitives
{
    public class Point2D : Primitive2D
    {
        private readonly Vector2D _originalVector;

        public Point2D(int x, int y, Color color) : this(new Vector2D(x, y), color)
        {
        }

        public Point2D(Vector2D v, Color color)
        {
            _originalVector = v;
            Vector = v;
            Color = color;
        }

        public int X => Vector.X;

        public int Y => Vector.Y;

        public Vector2D Vector { get; private set; }

        public Color Color { get; }

        public override Primitive2D Clone()
        {
            return new Point2D(_originalVector.X, _originalVector.Y, Color);
        }

        public override void Transform(Transform2D transform)
        {
            Vector = transform.Apply(_originalVector);
        }

        public override void Render(ICanvas2D canvas)
        {
            canvas.Set(Vector.X, Vector.Y, Color);
        }

        public override Primitive2Ds ToPrimitive2Ds()
        {
            return new Point2Ds
            {
                X = _originalVector.X,
                Y = _originalVector.Y,
                Color = Color.ToArgb(),
            };
        }
    }
}