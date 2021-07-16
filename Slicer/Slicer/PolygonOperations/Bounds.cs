// unset

using ClipperLib;
using Slicer.Models;
using System;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations
{
    public class Bounds
    {
        public static IntRect GetBounds(Polygons polygons)
        {
            long left = Int64.MaxValue;
            long right = Int64.MinValue;
            long top = Int64.MinValue;
            long bottom = Int64.MaxValue;

            foreach (IntRect rect in polygons.Select(GetBounds))
            {
                if (rect.left < left) left = rect.left;
                if (rect.right > right) right = rect.right;
                if (rect.top > top) top = rect.top;
                if (rect.bottom < bottom) bottom = rect.bottom;
            }

            return new IntRect(left, top, right, bottom);
        }

        public static IntRect GetBounds(Polygon polygon)
        {
            long left = Int64.MaxValue;
            long right = Int64.MinValue;
            long top = Int64.MinValue;
            long bottom = Int64.MaxValue;

            foreach (IntPoint point in polygon)
            {
                if (point.X > right) right = point.X;
                if (point.X < left) left = point.X;
                if (point.Y > top) top = point.Y;
                if (point.Y < bottom) bottom = point.Y;
            }

            return new IntRect(left, top, right, bottom);
        }
    }
}