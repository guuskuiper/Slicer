using ClipperLib;
using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Fill;
using Slicer.Slicer.PolygonOperations;
using System.Collections.Generic;
using Xunit;

namespace SlicerTests.Slicer.Fill
{
    public class ConcentricTests
    {
        [Fact]
        public void ConcentricFillTest()
        {
            // Arrange
            IOffset offset = new OffsetCopy();
            IConcentric concentric = new Concentric(offset);

            var contour = CreatePolygon.SquarePoly(10_000);

            var expectedPaths = new List<List<IntPoint>>
            {
                CreatePolygon.SquarePoly(9_000),
                CreatePolygon.SquarePoly(7_000),
                CreatePolygon.SquarePoly(5_000),
                CreatePolygon.SquarePoly(3_000),
                CreatePolygon.SquarePoly(1_000)
            };

            // Act
            var paths = concentric.ConcentricFill(contour, 1_000);

            // Assert 
            paths.Should().HaveSameCount(expectedPaths)
                .And.SatisfyRespectively(
                x0 => x0.Should().BeSubsetOf(expectedPaths[0]),
                x1 => x1.Should().BeSubsetOf(expectedPaths[1]),
                x2 => x2.Should().BeSubsetOf(expectedPaths[2]),
                x3 => x3.Should().BeSubsetOf(expectedPaths[3]),
                x4 => x4.Should().BeSubsetOf(expectedPaths[4])
            );
        }

        [Fact]
        public void ConcentricOutsideTest()
        {
            // Arrange
            IOffset offset = new OffsetCopy();
            IConcentric concentric = new Concentric(offset);

            var contour = CreatePolygon.SquarePoly(10_000);

            var expectedPaths = new List<List<IntPoint>>
            {
                CreatePolygon.SquarePoly(11_000),
                CreatePolygon.SquarePoly(13_000),
                CreatePolygon.SquarePoly(15_000),
            };

            // Act
            var paths = concentric.ConcentricOutside(new Polygons(new Polygon(contour)), 1_000, 3);

            // Assert 
            paths.Should().HaveSameCount(expectedPaths)
                .And.SatisfyRespectively(
                    x0 => x0.Should().BeSubsetOf(expectedPaths[0]),
                    x1 => x1.Should().BeSubsetOf(expectedPaths[1]),
                    x2 => x2.Should().BeSubsetOf(expectedPaths[2])
                );
        }
    }
}