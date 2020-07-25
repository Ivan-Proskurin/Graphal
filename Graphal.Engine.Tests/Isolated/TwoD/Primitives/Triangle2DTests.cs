using System.Drawing;

using FluentAssertions;

using Graphal.Engine.Tests.Mocks;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;

using NUnit.Framework;

namespace Graphal.Engine.Tests.Isolated.TwoD.Primitives
{
    public class Triangle2DTests
    {
        [Test]
        public void RenderTest()
        {
            var blackColor = Color.Black;
            var goldColor = Color.Gold;
            var canvas = new CanvasMock(20, 13, blackColor);

            var sut = new Triangle2D(
                new Vector2D(13, 10),
                new Vector2D(17, 8),
                new Vector2D(18, 11),
                goldColor);

            sut.Render(canvas);

            canvas[13, 8].Should().BeEquivalentTo(blackColor);
            canvas[14, 8].Should().BeEquivalentTo(blackColor);
            canvas[15, 8].Should().BeEquivalentTo(blackColor);
            canvas[16, 8].Should().BeEquivalentTo(goldColor);
            canvas[17, 8].Should().BeEquivalentTo(goldColor);
            canvas[18, 8].Should().BeEquivalentTo(blackColor);
            canvas[19, 8].Should().BeEquivalentTo(blackColor);

            canvas[13, 9].Should().BeEquivalentTo(blackColor);
            canvas[14, 9].Should().BeEquivalentTo(goldColor);
            canvas[15, 9].Should().BeEquivalentTo(goldColor);
            canvas[16, 9].Should().BeEquivalentTo(goldColor);
            canvas[17, 9].Should().BeEquivalentTo(goldColor);
            canvas[18, 9].Should().BeEquivalentTo(goldColor);
            canvas[19, 9].Should().BeEquivalentTo(blackColor);

            canvas[12, 10].Should().BeEquivalentTo(blackColor);
            canvas[13, 10].Should().BeEquivalentTo(goldColor);
            canvas[14, 10].Should().BeEquivalentTo(goldColor);
            canvas[15, 10].Should().BeEquivalentTo(goldColor);
            canvas[16, 10].Should().BeEquivalentTo(goldColor);
            canvas[17, 10].Should().BeEquivalentTo(goldColor);
            canvas[18, 10].Should().BeEquivalentTo(goldColor);
            canvas[19, 10].Should().BeEquivalentTo(blackColor);

            canvas[12, 11].Should().BeEquivalentTo(blackColor);
            canvas[13, 11].Should().BeEquivalentTo(blackColor);
            canvas[14, 11].Should().BeEquivalentTo(blackColor);
            canvas[15, 11].Should().BeEquivalentTo(goldColor);
            canvas[16, 11].Should().BeEquivalentTo(goldColor);
            canvas[17, 11].Should().BeEquivalentTo(goldColor);
            canvas[18, 11].Should().BeEquivalentTo(goldColor);
            canvas[19, 11].Should().BeEquivalentTo(blackColor);

            canvas[12, 12].Should().BeEquivalentTo(blackColor);
            canvas[13, 12].Should().BeEquivalentTo(blackColor);
            canvas[14, 12].Should().BeEquivalentTo(blackColor);
            canvas[15, 12].Should().BeEquivalentTo(blackColor);
            canvas[16, 12].Should().BeEquivalentTo(blackColor);
            canvas[17, 12].Should().BeEquivalentTo(blackColor);
            canvas[18, 12].Should().BeEquivalentTo(blackColor);
            canvas[19, 12].Should().BeEquivalentTo(blackColor);
        }
    }
}