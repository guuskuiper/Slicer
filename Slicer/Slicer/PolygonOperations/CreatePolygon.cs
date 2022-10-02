// unset

using ClipperLib;
using Slicer.Models;
using System;
using System.Collections.Generic;

namespace Slicer.Slicer.PolygonOperations
{
    public static class CreatePolygon
    {
        private static readonly IntPoint Origin = new IntPoint(0, 0);

        public static Polygon SquarePoly(long size) => SquarePoly(size, Origin);

        public static Polygon SquarePoly(long size, IntPoint center) => RectPoly(size, size, center);

        public static Polygon RectPoly(long width, long height) => RectPoly(width, height, Origin);

        public static Polygon RectPoly(long width, long height, IntPoint center)
        {
            var poly = new Polygon
            {
                new(center.X + width / 2, center.Y + height / 2),
                new(center.X - width / 2, center.Y + height / 2),
                new(center.X - width / 2, center.Y - height / 2),
                new(center.X + width / 2, center.Y - height / 2),
            };
            return poly;
        }
        
        public static Polygon CreateCircle(IntPoint center, double radius, int segments = 50, double startAngle = 0.0f)
        {
            Polygon polygon = new Polygon(segments+1);

            for (int i = 0; i < segments; i++)
            {
                double a = startAngle + 2.0f * Math.PI * i / segments;
                double x = Math.Cos(a) * radius;
                double y = Math.Sin(a) * radius;

                polygon.Add(new IntPoint(center.X + x, center.Y + y));
            }

            if (polygon.Count > 0) polygon.Add(polygon[0]);

            return polygon;
        }
    }
}