using Graphal.Engine.TwoD.Geometry;

namespace Graphal.Engine.TwoD.Transforms
{
    public class ShiftTransform2D : Transform2D
    {
        public ShiftTransform2D(int offsetX, int offsetY)
        {
            Offset = new Vector2D(offsetX, offsetY);
        }

        public Vector2D Offset { get; private set; }

        public override Vector2D Apply(Vector2D vector)
        {
            return vector + Offset;
        }

        public override Vector2D Invert(Vector2D vector)
        {
            return vector - Offset;
        }

        public override void Combine(Transform2D transform)
        {
            if (transform is ShiftTransform2D shiftTransform)
            {
                Offset += shiftTransform.Offset;
            }
        }
    }
}