// unset

using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.PolygonOperations;
using Slicer.Slicer.PolygonOperations.Triangulation;
using System;
using Xunit;

namespace SlicerTests.Slicer.PolygonOperations
{
    public class TriangulateTests
    {

        
        [Fact]
        public void TriangulateTest()
        {
            // Arrange
            Polygons polys = new Polygons()
            {
                new Polygon()
                {
                    new (0, 0), new (0, 11), new (10, 11), new (10, 0),
                }
            };

            Polygons expected = new Polygons()
            {
                new Polygon() {new (10, 11), new (0, 11), new (10, 0)},
                new Polygon() {new (10, 0), new (0, 11), new (0, 0),}
            };

            ITriangulate delaunayIncremental = new DelaunayIncremental();
            
            // Act
            var result = delaunayIncremental.Triangulate(polys);

            // Assert 
            result.Should().HaveSameCount(expected)
                .And.SatisfyRespectively(
                    x0 => x0.Should().BeEquivalentTo(expected[0]),
                    x1 => x1.Should().BeSubsetOf(expected[1]));
        }
        
        [Fact]
        public void TriangulateTest2()
        {
            // Arrange
            Polygons polys = new Polygons()
            {
                new Polygon()
                {
                    new (0, 0), new (0, 11), new (15, 5), new (10, 11), new (10, 0),
                }
            };

            ITriangulate delaunayIncremental = new DelaunayIncremental();
            
            // Act
            var result = delaunayIncremental.Triangulate(polys);

            // Assert 
            result.Should().HaveCount(3);
        }
        
        [Fact]
        public void TriangulateCircleTest()
        {
            // Arrange
            IOffset offset = new OffsetCopy();

            var polys = offset.PolyOffsetRound(new Polygon(CreatePolygon.SquarePoly(10)), 100_000);

            Console.WriteLine(polys[0].Count);
            
            ITriangulate delaunayIncremental = new DelaunayIncremental();
            
            // Act
            var result = delaunayIncremental.Triangulate(polys);

            // Assert 
            result.Should().HaveCount(polys[0].Count - 2);
        }
    }
}