// unset

using ClipperLib;
using Slicer.Models;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Slicer.Slicer.Slice
{
    public class Intersection
    {
        public List<Line> Intersections(float z, IEnumerable<STLTriangle> triangles)
        {
            List<Line> lines = new();

            foreach (var triangle in triangles)
            {
                if (LineAtZ(z, triangle.Vertex1, triangle.Vertex2, triangle.Vertex3, out Line l))
                {
                    lines.Add(l);
                }
            }

            return lines;
        }

        public bool LineAtZ(float z, Vector3 pt0, Vector3 pt1, Vector3 pt2, out Line line)
        {
            var i0 = PtAtZ(z, pt0, pt1);
            var i1 = PtAtZ(z, pt1, pt2);
            var i2 = PtAtZ(z, pt2, pt0);

            if (i0.HasValue && i1.HasValue && i2.HasValue)
            {
                line = null;
                return false;
            }

            if (i0.HasValue && i1.HasValue)
            {
                line = new Line(i0.Value, i1.Value);
                return true;
            }
            else if (i0.HasValue && i2.HasValue)
            {
                line = new Line(i0.Value, i2.Value);
                return true;
            }
            else if (i1.HasValue && i2.HasValue)
            {
                line = new Line(i1.Value, i2.Value);
                return true;
            }

            line = null;
            return false;
        }

        public IntPoint? PtAtZ(float z, Vector3 pt0, Vector3 pt1)
        {
            if (!InRange(z, pt0.Z, pt1.Z))
            {
                return null;
            }

            if (Math.Abs(z - pt0.Z) < 0.0001f)
            {
                return new IntPoint(pt0.X * 1000, pt0.Y * 1000);
            }
            
            if (Math.Abs(z - pt1.Z) < 0.0001f)
            {
                return new IntPoint(pt1.X * 1000, pt1.Y * 1000);
            }

            float x = pt0.X + (pt1.X - pt0.X) * (z - pt0.Z) / (pt1.Z - pt0.Z);
            float y = pt0.Y + (pt1.Y - pt0.Y) * (z - pt0.Z) / (pt1.Z - pt0.Z);
            return new IntPoint(x * 1000, y * 1000);
        }

        private bool InRange(float x, float a, float b) => 
            a <= b ? InRangeMinMax(x, a, b) : InRangeMinMax(x, b, a);

        private bool InRangeMinMax(float x, float min, float max) => 
            x >= min && x <= max;
    }
}