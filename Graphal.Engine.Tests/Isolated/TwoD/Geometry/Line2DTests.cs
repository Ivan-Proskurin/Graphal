using FluentAssertions;

using Graphal.Engine.TwoD.Geometry;

using NUnit.Framework;

namespace Graphal.Engine.Tests.Isolated.TwoD.Geometry
{
    public class Line2DTests
    {
        [Test]
        public void TestDecreasingOnZeroTest1()
        {
            var sut = new Line2D(new Vector2D(19, 2), new Vector2D(5, 6));

            sut.Test(5, 6, 0).Should().BeTrue();
            sut.Test(6, 5, 0).Should().BeFalse();
            sut.Test(7, 5, 0).Should().BeTrue();
            sut.Test(7, 6, 0).Should().BeTrue();
            sut.Test(8, 5, 0).Should().BeTrue();
            sut.Test(8, 6, 0).Should().BeFalse();
            sut.Test(15, 3, 0).Should().BeTrue();
            sut.Test(15, 4, 0).Should().BeFalse();
            sut.Test(16, 2, 0).Should().BeFalse();
        }
        
        [Test]
        public void TestDecreasingOnZeroTest2()
        {
            var sut = new Line2D(new Vector2D(5, 6), new Vector2D(19, 2));

            sut.Test(5, 6, 0).Should().BeTrue();
            sut.Test(6, 5, 0).Should().BeFalse();
            sut.Test(7, 5, 0).Should().BeTrue();
            sut.Test(7, 6, 0).Should().BeTrue();
            sut.Test(8, 5, 0).Should().BeTrue();
            sut.Test(8, 6, 0).Should().BeFalse();
            sut.Test(15, 3, 0).Should().BeTrue();
            sut.Test(15, 4, 0).Should().BeFalse();
            sut.Test(16, 2, 0).Should().BeFalse();
        }

        [Test]
        public void TestIncreasingOnZeroTest()
        {
            var sut = new Line2D(new Vector2D(2, 1), new Vector2D(14, 2));

            sut.Test(2, 1, 0).Should().BeTrue();
            sut.Test(3, 1, 0).Should().BeTrue();
            sut.Test(4, 1, 0).Should().BeTrue();
            sut.Test(6, 1, 0).Should().BeTrue();
            sut.Test(7, 1, 0).Should().BeTrue();
            sut.Test(8, 1, 0).Should().BeTrue();
            sut.Test(8, 2, 0).Should().BeTrue();
            sut.Test(9, 1, 0).Should().BeFalse();
            sut.Test(10, 1, 0).Should().BeFalse();
            sut.Test(11, 1, 0).Should().BeFalse();
            sut.Test(12, 1, 0).Should().BeFalse();
            sut.Test(9, 2, 0).Should().BeTrue();
            sut.Test(10, 2, 0).Should().BeTrue();
            sut.Test(14, 2, 0).Should().BeTrue();
        }
    }
}