using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.Clipper.Clipper2;
using Slicer.Slicer.PolygonOperations;
using System.Collections.Generic;
using Xunit;

namespace SlicerTests.Slicer.Clipper;

public class ClipTest : ClipTestsBase
{
    public ClipTest() : base(new Clip(new ClipperLib.Clipper()))
    {}
}

public class Clip2Test : ClipTestsBase
{
    public Clip2Test() : base(new Clip2())
    {}
}

public abstract class ClipTestsBase
{
    private readonly IClip clipper;

    public ClipTestsBase(IClip clip)
    {
        clipper = clip;
    }
        
    [Fact]
    public void DifferenceTest()
    {
        // Arrange
        Polygon subject = CreatePolygon.SquarePoly(10_000);
        Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new (0, -5_000));

        Polygon expected = CreatePolygon.RectPoly(10_000, 5_000, new (0, 5_000 / 2));

        // Act
        var result = clipper.Difference(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().ContainSingle()
            .Which.Should().HaveCount(4)
            .And.BeEquivalentTo(expected);
    }

    [Fact]
    public void DifferenceOpenTest()
    {
        // Arrange
        Polygon subject = new Polygon(new (5_000, 0), new (-5_000, 0));
        Polygon clip = CreatePolygon.RectPoly(2_000, 10_000);

        var expected =new List<List<Point2D>>
        {
            new List<Point2D>() { new(5_000, 0), new(1_000, 0) },
            new List<Point2D>() { new(-5_000, 0), new(-1_000, 0) },
        };

        // Act
        var result = clipper.DifferenceOpen(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().HaveCount(2)
            .And.BeEquivalentTo(expected);
    }

    [Fact]
    public void UnionTest()
    {
        // Arrange
        Polygon subject = CreatePolygon.SquarePoly(10_000);
        Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new (0, -5_000));

        Polygon expected = CreatePolygon.RectPoly(10_000, 15_000, new (0, -2_500));

        // Act
        var result = clipper.Union(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().ContainSingle()
            .Which.Should().HaveCount(4)
            .And.BeEquivalentTo(expected);
    }

    [Fact]
    public void UnionOpenTest()
    {
        // Arrange
        Polygon subject = new Polygon(new(5_000, 0), new(-5_000, 0));
        Polygon clip = CreatePolygon.RectPoly(2_000, 10_000);

        var expectedOpen = new List<List<Point2D>>
        {
            new List<Point2D>() { new(5_000, 0), new(1_000, 0) },
            new List<Point2D>() { new(-5_000, 0), new(-1_000, 0) },
        };

        Polygon expectedClosed = CreatePolygon.RectPoly(2_000, 10_000);

        // Act
        var result = clipper.UnionOpen(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.open.Should().HaveCount(2)
            .And.BeEquivalentTo(expectedOpen);
        result.closed.Should().ContainSingle()
            .Which.Should().HaveCount(4)
            .And.BeEquivalentTo(expectedClosed);
    }

    [Fact]
    public void IntersectionTest()
    {
        // Arrange
        //Polygon subject = CreatePolygon.SquarePoly(10_000);
        Polygon subject = CreatePolygon.RectPoly(10_000, 10_000);
        Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new (0, -5_000));

        Polygon expected = CreatePolygon.RectPoly(10_000, 5_000, new (0, -2_500));

        // Act
        var result = clipper.Intersection(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().ContainSingle()
            .Which.Should().HaveCount(4)
            .And.Contain(expected);
    }
    
    [Fact]
    public void Intersection2Test()
    {
        // Arrange
        Polygon subject = CreatePolygon.RectPoly(10_000, 10_000, new (0, 0));
        Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new (-5_000, 0));

        Polygon expected = CreatePolygon.RectPoly(5_000, 10_000, new (-2_500, 0));

        // Act
        var result = clipper.Intersection(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().ContainSingle()
            .Which.Should().HaveCount(4)
            .And.Contain(expected);
    }

    [Fact]
    public void IntersectionOpenTest()
    {
        // Arrange
        Polygon subject = new Polygon(new(5_000, 0), new(-5_000, 0));
        Polygon clip = CreatePolygon.RectPoly(2_000, 10_000);

        var expected = new List<List<Point2D>>
        {
            new() { new(1_000, 0), new(-1_000, 0) },
        };

        // Act
        var result = clipper.IntersectionOpen(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().ContainSingle()
            .And.BeEquivalentTo(expected);
    }

    [Fact]
    public void XorTest()
    {
        // Arrange
        Polygon subject = CreatePolygon.SquarePoly(10_000);
        Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new (0, -5_000));

        var expected = new List<List<Point2D>>
        {
            CreatePolygon.RectPoly(10_000, 5_000, new (0,  2_500)),
            CreatePolygon.RectPoly(10_000, 5_000, new (0, -7_500)),
        };

        // Act
        var result = clipper.Xor(new Polygons(subject), new Polygons(clip));

        // Assert 
        result.Should().HaveCount(2)
            .And.BeEquivalentTo(expected);
    }

    [Fact(Skip = "ClipperLib bug")]
    public void BugTest()
    {
        // Arrange
        Polygon subj = new Polygon()
        {
            new (65_000, 44_000),
            new (64_000, 44_000),
            //new (64_000, 44_001), //change previous point? Not horizontal anymore
            new (63_000, 43_000),
            new (63_000, 51_000)
        };

        Polygons clip = new Polygons()
        {
            new Polygon()
            {
                new (60_000, 50_000),
                new (70_000, 50_000),
                new (70_000, 40_000),
                new (60_000, 40_000)
            }
        };

        Polygon expected = new Polygon()
        {
            new (63_000, 50_000),
            new (63_000, 43_000),
            new (64_000, 44_000),
            new (65_000, 44_000)
        };

        // Act
        var result = clipper.IntersectionOpen(new Polygons(subj), clip);

        // Assert 
        result.Should().ContainSingle()
            .Which.Should().ContainInOrder(expected);
    }
}