using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.Fill;
using Slicer.Slicer.PolygonOperations;
using System;
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

            var expectedPaths = new List<List<Point2D>>
            {
                CreatePolygon.SquarePoly(9_000),
                CreatePolygon.SquarePoly(7_000),
                CreatePolygon.SquarePoly(5_000),
                CreatePolygon.SquarePoly(3_000),
                CreatePolygon.SquarePoly(1_000)
            };

            // Act
            var fillResult = concentric.ConcentricFill(contour, 1_000, Int32.MaxValue);

            // Assert 
            fillResult.Paths.Should().HaveSameCount(expectedPaths)
                .And.SatisfyRespectively(
                x0 => x0.Should().BeSubsetOf(expectedPaths[0]),
                x1 => x1.Should().BeSubsetOf(expectedPaths[1]),
                x2 => x2.Should().BeSubsetOf(expectedPaths[2]),
                x3 => x3.Should().BeSubsetOf(expectedPaths[3]),
                x4 => x4.Should().BeSubsetOf(expectedPaths[4])
            );
        }
        
        [Fact]
        public void ConcentricFillSinglePathTest()
        {
            // Arrange
            IOffset offset = new OffsetCopy();
            IConcentric concentric = new Concentric(offset);

            var contour = CreatePolygon.SquarePoly(10_000);

            var expectedPaths = new List<List<Point2D>>
            {
                CreatePolygon.SquarePoly(9_000),
            };
            
            var expectedRemaining = new List<List<Point2D>>
            {
                CreatePolygon.SquarePoly(8_000),
            };

            // Act
            var fillResult = concentric.ConcentricFill(contour, 1_000, 1);

            // Assert 
            fillResult.Paths.Should().HaveSameCount(expectedPaths)
                .And.SatisfyRespectively(
                    x0 => x0.Should().BeSubsetOf(expectedPaths[0])
                );
            fillResult.RemainingArea.Should().HaveSameCount(expectedRemaining)
                .And.SatisfyRespectively(
                    x0 => x0.Should().BeSubsetOf(expectedRemaining[0])
                );
        }

        [Fact]
        public void ConcentricOutsideTest()
        {
            // Arrange
            IOffset offset = new OffsetCopy();
            IConcentric concentric = new Concentric(offset);

            var contour = CreatePolygon.SquarePoly(10_000);

            var expectedPaths = new List<List<Point2D>>
            {
                CreatePolygon.SquarePoly(11_000),
                CreatePolygon.SquarePoly(13_000),
                CreatePolygon.SquarePoly(15_000),
            };

            // Act
            var fillResult = concentric.ConcentricOutside(new Polygons(new Polygon(contour)), 1_000, 3);

            // Assert 
            fillResult.Paths.Should().HaveSameCount(expectedPaths)
                .And.SatisfyRespectively(
                    x0 => x0.Should().BeSubsetOf(expectedPaths[0]),
                    x1 => x1.Should().BeSubsetOf(expectedPaths[1]),
                    x2 => x2.Should().BeSubsetOf(expectedPaths[2])
                );
        }
    }
}