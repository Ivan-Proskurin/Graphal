using System.Drawing;

using FluentAssertions;

using Graphal.Engine.TwoD.Geometry;

using NUnit.Framework;

namespace Graphal.Engine.Tests.Isolated.TwoD.Geometry
{
    public class EmbracingRectTests
    {
        [Test]
        public void ExtendByTest()
        {
            var sut = EmbracingRect.Empty;

            sut.ExtendBy(-1, -1);
            sut.ToRectangle()
                .Should()
                .BeEquivalentTo(new Rectangle(-1, -1, 1, 1));

            sut.ExtendBy(1, 1);
            sut.ToRectangle()
                .Should()
                .BeEquivalentTo(new Rectangle(-1, -1, 3, 3));

            sut.ExtendBy(-1, 1);
            sut.ToRectangle()
                .Should()
                .BeEquivalentTo(new Rectangle(-1, -1, 3, 3));

            sut.ExtendBy(-2, 1);
            sut.ToRectangle()
                .Should()
                .BeEquivalentTo(new Rectangle(-2, -1, 4, 3));

            sut.ExtendBy(0, 2);
            sut.ToRectangle()
                .Should()
                .BeEquivalentTo(new Rectangle(-2, -1, 4, 4));

            sut.Clear();
            sut.IsEmpty.Should().BeTrue();
        }
    }
}