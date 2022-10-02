using ClipperLib;
using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.Clipper.Clipper2;
using Slicer.Slicer.PolygonOperations;
using System;
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
            var poly = CreatePolygon.SquarePoly(10_000);

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
            var poly = CreatePolygon.SquarePoly(10_000);

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
            var poly = CreatePolygon.SquarePoly(10_000);

            // Act
            var offsetPolys = _offset.PolyOffset(poly, 1_000);

            // Assert
            offsetPolys.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 6_000 && Math.Abs(p.Y) == 6_000);
        }

        [Fact]
        public void PolyOffsets_ShouldAddPositiveOffset()
        {
            // Arrange
            var polys = CreatePolygon.SquarePoly(10_000);

            // Act
            var offsetPolys = _offset.PolyOffset(polys, 1_000);

            // Assert
            offsetPolys.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 6_000 && Math.Abs(p.Y) == 6_000);
        }

        [Fact]
        public void PolyOffsets_ShouldMergePositiveOffset()
        {
            // Arrange
            var polys = new Polygons(
                CreatePolygon.RectPoly(4_000, 10_000, new (-3_000, 0)),
                CreatePolygon.RectPoly(4_000, 10_000, new (+3_000, 0))
            );

            // Act
            var offsetPolys = _offset.PolyOffset(polys, 1_000);

            // Assert
            offsetPolys.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.OnlyContain(p => Math.Abs(p.X) == 6_000 && Math.Abs(p.Y) == 6_000);
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
    
    public class Offset2Tests : AbstractOffsetTest
    {
        public Offset2Tests() : base(new Offset2()) {}
    }
}