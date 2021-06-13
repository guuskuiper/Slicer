﻿using ClipperLib;
using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.PolygonOperations;
using System.Collections.Generic;
using Xunit;

namespace SlicerTests.Slicer.Clipper
{
    public class ClipTests
    {
        [Fact()]
        public void DifferenceTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = CreatePolygon.SquarePoly(10_000);
            Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000));

            Polygon expected = CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0, 5_000 / 2));

            // Act
            var result = clipper.Difference(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void DifferenceOpenTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(new IntPoint(5_000, 0), new IntPoint(-5_000, 0));
            Polygon clip = CreatePolygon.RectPoly(2_000, 10_000);

            var expected =new List<List<IntPoint>>
            {
                new List<IntPoint>() { new(5_000, 0), new(1_000, 0) },
                new List<IntPoint>() { new(-5_000, 0), new(-1_000, 0) },
            };

            // Act
            var result = clipper.DifferenceOpen(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Should().HaveCount(2)
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void UnionTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = CreatePolygon.SquarePoly(10_000);
            Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000));

            Polygon expected = CreatePolygon.RectPoly(10_000, 15_000, new IntPoint(0, -2_500));

            // Act
            var result = clipper.Union(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void UnionOpenTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(new(5_000, 0), new(-5_000, 0));
            Polygon clip = CreatePolygon.RectPoly(2_000, 10_000);

            var expectedOpen = new List<List<IntPoint>>
            {
                new List<IntPoint>() { new(5_000, 0), new(1_000, 0) },
                new List<IntPoint>() { new(-5_000, 0), new(-1_000, 0) },
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

        [Fact()]
        public void IntersectionTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = CreatePolygon.SquarePoly(10_000);
            Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000));

            Polygon expected = CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0, -2_500));

            // Act
            var result = clipper.Intersection(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Should().ContainSingle()
                .Which.Should().HaveCount(4)
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void IntersectionOpenTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = new Polygon(new(5_000, 0), new(-5_000, 0));
            Polygon clip = CreatePolygon.RectPoly(2_000, 10_000);

            var expected = new List<List<IntPoint>>
            {
                new List<IntPoint>() { new(1_000, 0), new(-1_000, 0) },
            };

            // Act
            var result = clipper.IntersectionOpen(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Should().ContainSingle()
                .And.BeEquivalentTo(expected);
        }

        [Fact()]
        public void XorTest()
        {
            // Arrange
            IClip clipper = new Clip(new ClipperLib.Clipper());
            Polygon subject = CreatePolygon.SquarePoly(10_000);
            Polygon clip = CreatePolygon.RectPoly(10_000, 10_000, new IntPoint(0, -5_000));

            var expected = new List<List<IntPoint>>
            {
                CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0,  2_500)),
                CreatePolygon.RectPoly(10_000, 5_000, new IntPoint(0, -7_500)),
            };
            // Act
            var result = clipper.Xor(new Polygons(subject), new Polygons(clip));

            // Assert 
            result.Should().HaveCount(2)
                .And.BeEquivalentTo(expected);
        }
    }
}