using ClipperLib;
using FluentAssertions;
using Slicer.Slicer.Clipper;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace SlicerTests.Slicer.Clipper
{
    public abstract class AbstractOffsetTest
    {
        private readonly IOffset _offset;

        public AbstractOffsetTest(IOffset offset)
        {
            _offset = offset;
        }

        [Fact]
        public void PolyOffset_ShouldAddNegativeOffset()
        {
            // Arrange
            List<IntPoint> poly = SquarePoly(10_000);

            // Act
            var offsetPolys = _offset.PolyOffset(poly, -1_000);

            // Assert
            offsetPolys.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 4_000 && Math.Abs(p.Y) == 4_000);
        }

        [Fact]
        public void PolyOffset_ShouldAddPositiveOffset()
        {
            // Arrange
            var poly = SquarePoly(10_000);

            // Act
            var offsetPolys = _offset.PolyOffset(poly, 1_000);

            // Assert
            offsetPolys.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 6_000 && Math.Abs(p.Y) == 6_000);
        }

        private static List<IntPoint> SquarePoly(long size)
        {
            long halfSize = size / 2;
            var poly = new List<IntPoint>()
            {
                new(halfSize, halfSize),
                new(-halfSize, halfSize),
                new(-halfSize, -halfSize),
                new(halfSize, -halfSize),
            };
            return poly;
        }

    }

    public class OffsetCopyTests : AbstractOffsetTest
    {
        public OffsetCopyTests() : base(new OffsetCopy()) {}
    }

    public class OffsetTests : AbstractOffsetTest
    {
        public OffsetTests() : base(new Offset(new ClipperOffset())) {}
    }
}