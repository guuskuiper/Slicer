using Slicer.Models;
using System;
using System.Collections.Generic;

namespace Slicer.Slicer.PolygonOperations
{
    //using Polygon = List<IntPoint>;
    //using Polygons = List<List<IntPoint>>;

    public static class Simplify
    {
        private static double GetX(Point2D pt) => pt.X;
        private static double GetY(Point2D pt) => pt.Y;

        /// <summary>
        /// Reduce the number of points in "poly".
        /// </summary>
        /// <param name="poly">A polygon</param>
        /// <param name="epsilon_um">The maximum deviation from the original path</param>
        /// <param name="useStack">Enable to use a stack in stead of recursion (and avoid the recursion limit for large polys) </param>
        /// <param name="usePreprocessing">First use a fast, but low quality preprocessing step to simplify the poly</param>
        /// <returns></returns>
        public static Polygon Reduce(Polygon poly, long epsilon_um, bool useStack = true, bool usePreprocessing = false)
        {
            return new Polygon(DouglasPeucker<Point2D>.Reduce(poly.AsReadOnly(), epsilon_um, GetX, GetY, useStack, usePreprocessing));
        }

        /// <summary>
        /// Reduce the number of points in "polys".
        /// </summary>
        /// <param name="polys">A list of polygons</param>
        /// <param name="epsilon_um">The maximum deviation from the original path</param>
        /// <param name="useStack">Enable to use a stack in stead of recursion (and avoid the recursion limit for large polys) </param>
        /// <param name="usePreprocessing">First use a fast, but low quality preprocessing step to simplify the poly</param>
        /// <returns></returns>
        public static Polygons Reduce(Polygons polys, long epsilon_um, bool useStack = true, bool usePreprocessing = false)
        {
            var reducedPolys = new Polygons(polys.Count);
            foreach (var poly in polys)
            {
                reducedPolys.Add(Reduce(poly, epsilon_um, useStack, usePreprocessing));
            }
            return reducedPolys;
        }

        /// <summary>
        /// A function to get the squared distance from point x0, y0 to x1, y1
        /// </summary>
        /// <returns></returns>
        public static double PointDistanceSquared(double x0, double y0, double x1, double y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// A function to get the squared distance from point ptX, ptY to line segment ln0X, ln0Y - ln1X, ln1Y 
        /// </summary>
        /// <param name="ptX">pt X</param>
        /// <param name="ptY">pt Y</param>
        /// <param name="ln0X">ln0 X</param>
        /// <param name="ln0Y">ln0 Y</param>
        /// <param name="ln1X">ln1 X</param>
        /// <param name="ln1Y">ln1 Y</param>
        /// <param name="ptNearX">nearest X on the line segment</param>
        /// <param name="ptNearY">nearest Y on the line segment</param>
        /// <returns></returns>
        public static double LineSegmentDistanceSquared(double ptX, double ptY, double ln0X, double ln0Y, double ln1X, double ln1Y, out double ptNearX, out double ptNearY)
        {
            double dx = ln1X - ln0X;
            double dy = ln1Y - ln0Y;

            if (dx == 0 && dy == 0) // Not a line segment.
            {
                ptNearX = ln0X;
                ptNearY = ln0Y;

                dx = ptX - ln0X;
                dy = ptY - ln0Y;
            }
            else
            {
                double numerator = (ptX - ln0X) * dx + (ptY - ln0Y) * dy;
                double denom = dx * dx + dy * dy;
                double t = numerator / denom;

                if (t < 0)
                {
                    ptNearX = ln0X;
                    ptNearY = ln0Y;

                    dx = ptX - ln0X;
                    dy = ptY - ln0Y;
                }
                else if (t > 1)
                {
                    ptNearX = ln1X;
                    ptNearY = ln1Y;

                    dx = ptX - ln1X;
                    dy = ptY - ln1Y;
                }
                else
                {
                    ptNearX = ln0X + t * dx;
                    ptNearY = ln0Y + t * dy;

                    dx = ptX - ptNearX;
                    dy = ptY - ptNearY;
                }
            }
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// A function to get the squared distance from point ptX, ptY, ptZ to line segment ln0X, ln0Y, ln0Z - ln1X, ln1Y, ln1Z
        /// </summary>
        /// <param name="ptX">pt X</param>
        /// <param name="ptY">pt Y</param>
        /// <param name="ptZ">pt Z</param>
        /// <param name="ln0X">ln0 X</param>
        /// <param name="ln0Y">ln0 Y</param>
        /// <param name="ln0Z">ln0 Z</param>
        /// <param name="ln1X">ln1 X</param>
        /// <param name="ln1Y">ln1 Y</param>
        /// <param name="ln1Z">ln1 Z </param>
        /// <param name="ptNearX">nearest X on the line segment</param>
        /// <param name="ptNearY">nearest Y on the line segment</param>
        /// <param name="ptNearZ">nearest Z on the line segment</param>
        /// <returns></returns>
        public static double LineSegmentDistanceSquared(double ptX, double ptY, double ptZ, double ln0X, double ln0Y, double ln0Z, double ln1X, double ln1Y, double ln1Z, out double ptNearX, out double ptNearY, out double ptNearZ)
        {
            double dx = ln1X - ln0X;
            double dy = ln1Y - ln0Y;
            double dz = ln1Z - ln0Z;

            if (dx == 0 && dy == 0 && dz == 0) // Not a line segment.
            {
                ptNearX = ln0X;
                ptNearY = ln0Y;
                ptNearZ = ln0Z;

                dx = ptX - ln0X;
                dy = ptY - ln0Y;
            }
            else
            {
                double numerator = (ptX - ln0X) * dx + (ptY - ln0Y) * dy + (ptZ - ln0Z) * dz;
                double denom = dx * dx + dy * dy + dz * dz;
                double t = numerator / denom;

                if (t < 0)
                {
                    ptNearX = ln0X;
                    ptNearY = ln0Y;
                    ptNearZ = ln0Z;

                    dx = ptX - ln0X;
                    dy = ptY - ln0Y;
                    dz = ptZ - ln0Z;
                }
                else if (t > 1)
                {
                    ptNearX = ln1X;
                    ptNearY = ln1Y;
                    ptNearZ = ln1Z;

                    dx = ptX - ln1X;
                    dy = ptY - ln1Y;
                    dz = ptZ - ln1Z;
                }
                else
                {
                    ptNearX = ln0X + t * dx;
                    ptNearY = ln0Y + t * dy;
                    ptNearZ = ln0Z + t * dz;

                    dx = ptX - ptNearX;
                    dy = ptY - ptNearY;
                    dz = ptZ - ptNearZ;
                }
            }
            return dx * dx + dy * dy + dz * dz;
        }
    }

    public class DouglasPeucker<T>
    {
        private readonly IReadOnlyList<T> Poly;
        private readonly bool[] Remove;
        private readonly double Epsilon_um2;
        private readonly Stack<(int, int)> Stack;
        private readonly bool UseStack;
        private readonly Func<T, T, T, double> SquaredDistanceFunc;

        /// <summary>
        /// Reduce the number of points in "poly".
        /// </summary>
        /// <param name="poly">A list of T</param>
        /// <param name="epsilon_um">The maximum deviation from the original path</param>
        /// <param name="squaredLineDistanceFunc">A function to get the squared distance from point T1 to line segment T2 - T3</param>
        /// <param name="useStack">Enable to use a stack in stead of recursion (and avoid the recursion limit for large polys) </param>
        /// <returns></returns>
        public static IReadOnlyList<T> Reduce(IReadOnlyList<T> poly, double epsilon_um, Func<T, T, T, double> squaredLineDistanceFunc, bool useStack = true)
        {
            var epsilon_um2 = epsilon_um * epsilon_um;
            var dp = new DouglasPeucker<T>(poly, epsilon_um2, squaredLineDistanceFunc, useStack);
            return dp.Result();
        }

        /// <summary>
        /// Reduce the number of points in "poly".
        /// </summary>
        /// <param name="poly">A list of T</param>
        /// <param name="epsilon_um">The maximum deviation from the original path</param>
        /// <param name="squaredLineDistanceFunc">A function to get the squared distance from point T1 to line segment T2 - T3</param>
        /// <param name="squaredDistanceFunc">A function to get the squared distance from point T1 to point T2, used in the preprocessing function</param>
        /// <param name="useStack">Enable to use a stack in stead of recursion (and avoid the recursion limit for large polys) </param>
        /// <returns></returns>
        public static IReadOnlyList<T> Reduce(IReadOnlyList<T> poly, double epsilon_um, Func<T, T, T, double> squaredLineDistanceFunc, Func<T, T, double> squaredDistanceFunc, bool useStack = true)
        {
            var epsilon_um2 = epsilon_um * epsilon_um;
            poly = PreProcess(poly, epsilon_um2, squaredDistanceFunc);
            var dp = new DouglasPeucker<T>(poly, epsilon_um2, squaredLineDistanceFunc, useStack);
            return dp.Result();
        }

        /// <summary>
        /// Reduce the number of points in "poly".
        /// </summary>
        /// <param name="poly">A list of T</param>
        /// <param name="epsilon_um">The maximum deviation from the original path</param>
        /// <param name="getX">A function to get the X coordinate from T</param>
        /// <param name="getY">A function to get the Y coordinate from T</param>
        /// <param name="useStack">Enable to use a stack in stead of recursion (and avoid the recursion limit for large polys) </param>
        /// <param name="usePreprocessing">First use a fast, but low quality preprocessing step to simplify the poly</param>
        /// <returns></returns>
        public static IReadOnlyList<T> Reduce(IReadOnlyList<T> poly, double epsilon_um, Func<T, double> getX, Func<T, double> getY, bool useStack = true, bool usePreprocessing = false)
        {
            double squaredLineDistance(T pt, T l0, T l1) => Simplify.LineSegmentDistanceSquared(getX(pt), getY(pt), getX(l0), getY(l0), getX(l1), getY(l1), out _, out _);
            var epsilon_um2 = epsilon_um * epsilon_um;
            if (usePreprocessing)
            {
                double squaredDistance(T p0, T p1) => Simplify.PointDistanceSquared(getX(p0), getY(p0), getX(p1), getY(p1));
                poly = PreProcess(poly, epsilon_um2, squaredDistance);
            }
            var dp = new DouglasPeucker<T>(poly, epsilon_um2, squaredLineDistance, useStack);
            return dp.Result();
        }

        private DouglasPeucker(IReadOnlyList<T> poly, double epsilon_um2, Func<T, T, T, double> squaredDistanceFunc, bool useStack)
        {
            Poly = poly;
            Remove = new bool[Poly.Count];
            Epsilon_um2 = epsilon_um2;
            UseStack = useStack;
            SquaredDistanceFunc = squaredDistanceFunc;
            if (useStack) Stack = new Stack<(int, int)>();
        }

        private IReadOnlyList<T> Result()
        {
            if (Poly.Count < 3) return Poly;

            if (UseStack)
            {
                Stack.Push((0, Poly.Count - 1));
                while (Stack.Count != 0)
                {
                    var s = Stack.Pop();
                    DouglasPeuckerSection(s.Item1, s.Item2);
                }
            }
            else
            {
                DouglasPeuckerSection(0, Poly.Count - 1);
            }

            List<T> simplified = new List<T>();
            for (int i = 0; i < Poly.Count; i++)
            {
                if (!Remove[i])
                {
                    simplified.Add(Poly[i]);
                }
            }

            return simplified;
        }

        private void DouglasPeuckerSection(int i, int j)
        {
            if (i + 1 == j) return;

            double dmaxSq = 0;
            int index = -1;

            var first = Poly[i];
            var last = Poly[j];

            for (int k = i + 1; k < j; k++)
            {
                var dSq = SquaredDistanceFunc(Poly[k], first, last);
                if (dSq > dmaxSq)
                {
                    index = k;
                    dmaxSq = dSq;
                }
            }

            if (dmaxSq > Epsilon_um2)
            {
                /* Recursive simplify */
                if (UseStack)
                {
                    Stack.Push((i, index));
                    Stack.Push((index, j));
                }
                else
                {
                    DouglasPeuckerSection(i, index);
                    DouglasPeuckerSection(index, j);
                }

            }
            else
            {
                for (int k = i + 1; k < j; k++)
                {
                    Remove[k] = true;
                }
            }
        }

        /// <summary>
        /// Reduce the number of points in "poly" in a fast, but low quality way.
        /// </summary>
        /// <param name="poly">A list of T</param>
        /// <param name="epsilon_um2">The minimum squared distance between points</param>
        /// <param name="getX">A function to get the X coordinate from T</param>
        /// <param name="getY">A function to get the Y coordinate from T</param>
        /// <returns></returns>
        private static IReadOnlyList<T> PreProcess(IReadOnlyList<T> poly, double epsilon_um2, Func<T, T, double> getDistanceSquared)
        {
            if (poly.Count < 3) return poly;

            T prevPt = poly[0];
            T curPt = poly[0];
            List<T> newPoly = new List<T> { prevPt };

            for (int i = 1; i < poly.Count; ++i)
            {
                curPt = poly[i];
                if (getDistanceSquared(curPt, prevPt) > epsilon_um2)
                {
                    newPoly.Add(curPt);
                    prevPt = curPt;
                }
            }

            if (!curPt.Equals(prevPt))
            {
                newPoly.Add(curPt);
            }

            return newPoly;
        }
    }
}