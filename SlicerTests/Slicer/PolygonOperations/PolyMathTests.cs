﻿// unset

using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.PolygonOperations;
using Xunit;

namespace SlicerTests.Slicer.PolygonOperations
{
    public class PolyMathTests
    {
                [Theory]
                [InlineData(-1, -1, true)]
                [InlineData(-1, -9, true)]
                [InlineData(-1, 12, true)]
                [InlineData(-1, 10, false)]
                [InlineData(-1, 1, false)]
                [InlineData(9, 9, false)]
                [InlineData(5, -1, false)]
                [InlineData(12, -1, true)]
                [InlineData(12, -5, false)]
                [InlineData(0, 0, true)]  // point in a line is in the out region
                [InlineData(10, 0, true)] // point in a line is in the out region
                [InlineData(0, 10, true)] // point in a line is in the out region
                [InlineData(-1, 0, true)] // point in a line is in the out region
                [InlineData(1, 1, false)] // inside triangle
                public void IsPointInOutRegionTest(long x, long y, bool inRegion)
                {
                    // Arrange
                    Point2D A = new (0, 0);
                    Point2D B = new (10, 0);
                    Point2D C = new (0, 10);
        
                    Point2D pt = new (x, y);
                    
                    // Act
                    bool ccw = PolyMath.IsCCW(A, B, C);
                    bool inOutRegion = PolyMath.IsPointInOutRegion(A, B, C, pt);
        
                    // Assert 
                    inOutRegion.Should().Be(inRegion);
                    ccw.Should().BeTrue();
                }
                
                [Theory]
                [InlineData(1, 1, true)] // inside triangle
                [InlineData(6, 6, true)]
                [InlineData(-1, 5, true)]
                [InlineData(-1, 11, false)]
                [InlineData(5, 20, false)]
                [InlineData(5, -5, false)]
                [InlineData(5, -1, true)]
                public void IsPointInCircle(long x, long y, bool inRegion)
                {
                    // Arrange
                    Point2D A = new (0, 0);
                    Point2D B = new (10, 0);
                    Point2D C = new (0, 10);
                    Triangle t = new Triangle(A, B, C);
        
                    Point2D pt = new (x, y);
                        
                    // Act
                    bool inOutRegion = PolyMath.PointInCircle(pt, t);
        
                    // Assert 
                    inOutRegion.Should().Be(inRegion);
                }
                
                [Fact]
                public void IsCCW()
                {
                    // Arrange
                    Point2D A = new (0, 0);
                    Point2D B = new (10, 0);
                    Point2D C = new (0, 10);
                        
                    // Act
                    bool ccw = PolyMath.IsCCW(A, B, C);
        
                    // Assert 
                    ccw.Should().BeTrue();
                }
                
                [Fact]
                public void IsCCW2()
                {
                    // Arrange
                    Point2D A = new (0, 0);
                    Point2D B = new (10, 0);
                    Point2D C = new (0, 10);
                        
                    // Act
                    bool ccw = PolyMath.IsCCW(C, A, B);
        
                    // Assert 
                    ccw.Should().BeTrue();
                }
                
                [Fact]
                public void IsCW()
                {
                    // Arrange
                    Point2D A = new (0, 0);
                    Point2D B = new (10, 0);
                    Point2D C = new (0, 10);
                        
                    // Act
                    bool ccw = PolyMath.IsCCW(B, A, C);
        
                    // Assert 
                    ccw.Should().BeFalse();
                }
                
                [Fact]
                public void IsCW2()
                {
                    // Arrange
                    Point2D A = new (0, 0);
                    Point2D B = new (10, 0);
                    Point2D C = new (0, 10);
                        
                    // Act
                    bool ccw = PolyMath.IsCCW(C, B, A);
        
                    // Assert 
                    ccw.Should().BeFalse();
                }
    }
}