using ClipperLib;
using FluentAssertions;
using Slicer.Models;
using Xunit;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.PolygonOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Slicer.Clipper.Tests
{
    public class ClipTests
    {
        [Fact()]
        public void DifferenceTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(CreatePolygon.SquarePoly(10_000));
            Polygon clip = new Polygon(CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000)));

            Polygon expected = new Polygon(CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0, 5_000 / 2)));

            // Act
            var result = clipper.Difference(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
                .And.BeEquivalentTo(expected.Poly);
        }

        [Fact()]
        public void DifferenceOpenTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(new List<IntPoint> { new (5_000, 0), new (-5_000, 0) });
            Polygon clip = new Polygon(CreatePolygon.RectPoly(2_000, 10_000));

            var expected =new List<List<IntPoint>>
            {
                new List<IntPoint>() { new(5_000, 0), new(1_000, 0) },
                new List<IntPoint>() { new(-5_000, 0), new(-1_000, 0) },
            };

            // Act
            var result = clipper.DifferenceOpen(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.GetPolys().Should().HaveCount(2)
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void UnionTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(CreatePolygon.SquarePoly(10_000));
            Polygon clip = new Polygon(CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000)));

            Polygon expected = new Polygon(CreatePolygon.RectPoly(10_000, 15_000, new IntPoint(0, -2_500)));

            // Act
            var result = clipper.Union(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
                .And.BeEquivalentTo(expected.Poly);
        }

        [Fact()]
        public void UnionOpenTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(new List<IntPoint> { new(5_000, 0), new(-5_000, 0) });
            Polygon clip = new Polygon(CreatePolygon.RectPoly(2_000, 10_000));

            var expectedOpen = new List<List<IntPoint>>
            {
                new List<IntPoint>() { new(5_000, 0), new(1_000, 0) },
                new List<IntPoint>() { new(-5_000, 0), new(-1_000, 0) },
            };

            Polygon expectedClosed = new Polygon(CreatePolygon.RectPoly(2_000, 10_000));

            // Act
            var result = clipper.UnionOpen(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.open.GetPolys().Should().HaveCount(2)
                .And.BeEquivalentTo(expectedOpen);
            result.closed.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
                .And.BeEquivalentTo(expectedClosed.Poly);
        }

        [Fact()]
        public void IntersectionTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(CreatePolygon.SquarePoly(10_000));
            Polygon clip = new Polygon(CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000)));

            Polygon expected = new Polygon(CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0, -2_500)));

            // Act
            var result = clipper.Intersection(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Polys.Should().ContainSingle()
                .Which.Poly.Should().HaveCount(4)
                .And.BeEquivalentTo(expected.Poly);
        }

        [Fact()]
        public void IntersectionOpenTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(new List<IntPoint> { new(5_000, 0), new(-5_000, 0) });
            Polygon clip = new Polygon(CreatePolygon.RectPoly(2_000, 10_000));

            var expected = new List<List<IntPoint>>
            {
                new List<IntPoint>() { new(1_000, 0), new(-1_000, 0) },
            };

            // Act
            var result = clipper.IntersectionOpen(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.GetPolys().Should().ContainSingle()
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void XorTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(CreatePolygon.SquarePoly(10_000));
            Polygon clip = new Polygon(CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000)));

            var expected = new List<List<IntPoint>>
            {
                CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0,  2_500)),
                CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0, -7_500)),
            };
            // Act
            var result = clipper.Xor(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.GetPolys().Should().HaveCount(2)
                .And.BeEquivalentTo(expected);
        }
    }
}