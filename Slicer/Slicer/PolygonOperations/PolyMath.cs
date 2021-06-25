// unset

using ClipperLib;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Slicer.Slicer.PolygonOperations
{
    public record Triangle(IntPoint P0, IntPoint P1, IntPoint P2)
    {
        public bool Contains(IntPoint p)
        {
            return P0 == p || P1 == p || P2 == p;
        }

        public IntPoint GetOther(IntPoint p0, IntPoint p1)
        {
            List<IntPoint> points = new() {this.P0, this.P1, this.P2};
            return points.SingleOrDefault(p => p != p0 && p != p1);
        }
    }
    
    public static class PolyMath
    {
        public static bool PointInTriangle(IntPoint p, Triangle t) => PointInTriangle(p, t.P0, t.P1, t.P2);
                
                //https://stackoverflow.com/a/23186198/14265807
                public static bool PointInTriangle(IntPoint p, IntPoint p0, IntPoint p1, IntPoint p2)
                {
                    var s = p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y;
                    var t = p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y;
        
                    if ((s < 0) != (t < 0))
                        return false;
        
                    var A = -p1.Y * p2.X + p0.Y * (p2.X - p1.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y;
        
                    return A < 0 ?
                        (s <= 0 && s + t >= A) :
                        (s >= 0 && s + t <= A);
                    //return (s + t) < A;
                }
        
                public static bool PointInCircle(IntPoint d, Triangle t)
                {
                    if (PointInTriangle(d, t))
                    {
                        return true;
                    }
        
                    Triangle tt;
                    if (IsCCW(t))
                    {
                        tt = t;
                    }
                    else
                    {
                        tt = new Triangle(t.P1, t.P0, t.P2);
                    }
        
                    return InCircle(d, tt);
                }
        
                private static bool InCircle(IntPoint d, Triangle t)
                {
                    // t should be a CCW triangle
                    Matrix4x4 matrix4X4 = new Matrix4x4(
                        t.P0.X, t.P0.Y, t.P0.X * t.P0.X + t.P0.Y * t.P0.Y, 1,
                        t.P1.X, t.P1.Y, t.P1.X * t.P1.X + t.P1.Y * t.P1.Y, 1,
                        t.P2.X, t.P2.Y, t.P2.X * t.P2.X + t.P2.Y * t.P2.Y, 1,
                        d.X, d.Y, d.X * d.X + d.Y * d.Y, 1);
                    return matrix4X4.GetDeterminant() > 0;
                }
        
                public static long IsPointLeftOfLine(IntPoint p0, IntPoint p1, IntPoint p2)
                {
                    var criterium = (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
        
                    return criterium;
                    //if (criterium >= 0) return true;
                    //return false;
                }
        
                public static  bool IsPointInOutRegion(IntPoint A, IntPoint B, IntPoint C, IntPoint point)
                {
                    // Points A, B, and C form a CCW triangle
        
                    return ((IsPointLeftOfLine(A, C, point) >= 0 && IsPointLeftOfLine(B, C, point) <= 0)
                            || (IsPointLeftOfLine(C, B, point) >= 0 && IsPointLeftOfLine(A, B, point) <= 0)
                            || (IsPointLeftOfLine(B, A, point) >=0 && IsPointLeftOfLine(C, A, point) <= 0));
                }
        
                public static bool IsCCW(Triangle t) => IsCCW(t.P0, t.P1, t.P2);
        
                public static bool IsCCW(IntPoint A, IntPoint B, IntPoint C)
                {
                    // (x2 - x1)(y2 + y1)
                    long sum = 0;
                    
                    sum += (B.X - A.X) * (B.Y + A.Y);
                    sum += (C.X - B.X) * (C.Y + B.Y);
                    sum += (A.X - C.X) * (A.Y + C.Y);
                    
                    return sum < 0;
                }
    }
}