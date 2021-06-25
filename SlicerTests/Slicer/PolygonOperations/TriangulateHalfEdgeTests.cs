// unset

using ClipperLib;
using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.PolygonOperations;
using Xunit;
using Triangle = Slicer.Slicer.PolygonOperations.Triangle;

namespace SlicerTests.Slicer.PolygonOperations
{
    public class TriangulateHalfEdgeTests
    {

        
        [Fact]
        public void TriangulateTest()
        {
            // Arrange
            Polygons polys = new Polygons()
            {
                new Polygon()
                {
                    new IntPoint(0, 0), new IntPoint(0, 11), new IntPoint(10, 11), new IntPoint(10, 0),
                }
            };

            Polygons expected = new Polygons()
            {
                new Polygon() {new IntPoint(10, 11), new IntPoint(0, 11), new IntPoint(10, 0)},
                new Polygon() {new IntPoint(10, 0), new IntPoint(0, 11), new IntPoint(0, 0),}
            };

            ITriangulate delaunayIncremental = new DelaunayIncrementalHalfEdge();
            
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
                    new IntPoint(0, 0), new IntPoint(0, 11), new IntPoint(15, 5), new IntPoint(10, 11), new IntPoint(10, 0),
                }
            };

            ITriangulate delaunayIncremental = new DelaunayIncrementalHalfEdge();
            
            // Act
            var result = delaunayIncremental.Triangulate(polys);

            // Assert 
            result.Should().HaveCount(3);
        }
    }
}