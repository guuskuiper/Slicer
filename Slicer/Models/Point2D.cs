using System;

namespace Slicer.Models;

public struct Point2D
{
    public readonly long X;
    public readonly long Y;

    public Point2D(long x, long y)
    {
        X = x;
        Y = y;
    }

    public Point2D(Point2D point)
    {
        X = point.X;
        Y = point.Y;
    }
    
    public Point2D(double x, double y)
    {
        this.X = (long) Math.Round(x);
        this.Y = (long) Math.Round(y);
    }
    
    public static bool operator ==(Point2D lhs, Point2D rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;

    public static bool operator !=(Point2D lhs, Point2D rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;

    public static Point2D operator +(Point2D lhs, Point2D rhs) => new Point2D(lhs.X + rhs.X, lhs.Y + rhs.Y);

    public static Point2D operator -(Point2D lhs, Point2D rhs) => new Point2D(lhs.X - rhs.X, lhs.Y - rhs.Y);

    public override string ToString() => string.Format("{0},{1} ", (object) this.X, (object) this.Y);

    public override bool Equals(object? obj) => obj != null && obj is Point2D Point2D && this == Point2D;

    public override int GetHashCode() => 0;
}