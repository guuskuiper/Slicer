using ClipperLib;
using FluentAssertions;
using Slicer.Models;
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
        public void ListIntPointOffset_ShouldAddNegativeOffset()
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
        public void ListIntPointOffset_ShouldAddPositiveOffset()
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

        [Fact]
        public void PolyOffset_ShouldAddPositiveOffset()
        {
            // Arrange
            var poly = new Polygon(SquarePoly(10_000));

            // Act
            var offsetPolys = _offset.PolyOffset(poly, 1_000);

            // Assert
            offsetPolys.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 6_000 && Math.Abs(p.Y) == 6_000);
        }

        [Fact]
        public void PolyOffsets_ShouldAddPositiveOffset()
        {
            // Arrange
            var polys = new Polygons(new Polygon(SquarePoly(10_000)));

            // Act
            var offsetPolys = _offset.PolyOffset(polys, 1_000);

            // Assert
            offsetPolys.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 6_000 && Math.Abs(p.Y) == 6_000);
        }

        [Fact]
        public void PolyOffsets_ShouldMergePositiveOffset()
        {
            // Arrange
            var polys = new Polygons(new List<List<IntPoint>>()
            {
                RectPoly(4_000, 10_000, -3_000),
                RectPoly(4_000, 10_000, +3_000),
            });

            // Act
            var offsetPolys = _offset.PolyOffset(polys, 1_000);

            // Assert
            offsetPolys.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
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

        private static List<IntPoint> RectPoly(long width, long height, long cX = 0, long cY = 0)
        {
            var poly = new List<IntPoint>()
            {
                new(cX + width / 2, cY + height / 2),
                new(cX - width / 2, cY + height / 2),
                new(cX - width / 2, cY - height / 2),
                new(cX + width / 2, cY - height / 2),
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