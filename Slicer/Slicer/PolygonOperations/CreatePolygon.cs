// unset

using ClipperLib;
using System.Collections.Generic;

namespace Slicer.Slicer.PolygonOperations
{
    public static class CreatePolygon
    {
        private static readonly IntPoint Origin = new IntPoint(0, 0);

        public static List<IntPoint> SquarePoly(long size) => SquarePoly(size, Origin);

        public static List<IntPoint> SquarePoly(long size, IntPoint center) => RectPoly(size, size, center);

        public static List<IntPoint> RectPoly(long width, long height) => RectPoly(width, height, Origin);

        public static List<IntPoint> RectPoly(long width, long height, IntPoint center)
        {
            var poly = new List<IntPoint>()
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