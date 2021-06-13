// unset

using ClipperLib;
using Slicer.Models;
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
    }
}