using FluentAssertions;

using Graphal.Engine.TwoD.Geometry;

using NUnit.Framework;

namespace Graphal.Engine.Tests.Isolated.TwoD.Geometry
{
    public class Vector2DTests
    {
        [TestCase(20, 15, 17)]
        [TestCase(20, 17, 15)]
        [TestCase(12, 18, 15)]
        [TestCase(10, 10, 10)]
        [TestCase(17, 15, 20)]
        [TestCase(11, 15, 20)]
        public void SortByTest(int y1, int y2, int y3)
        {
            var v1 = new Vector2D(12, y1);
            var v2 = new Vector2D(15, y2);
            var v3 = new Vector2D(2, y3);

            Vector2D.SortByY(ref v1, ref v2, ref v3);

            v1.Y.Should().BeLessOrEqualTo(v2.Y);
            v2.Y.Should().BeLessOrEqualTo(v3.Y);
        }
    }
}