using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Clipper.Clipper1;

public static class Clipper1Converters
{
    public static List<IntPoint> ToPath(Polygon poly)
    {
        List<IntPoint> path = new(poly.Count);
        foreach (var point in poly)
        {
            path.Add(new (point.X, point.Y));
        }

        return path;
    }
    
    public static List<List<IntPoint>> ToPaths(Polygons polys)
    {
        List<List<IntPoint>> paths = new(polys.Count);
        foreach (var poly in polys)
        {
            paths.Add(ToPath(poly));
        }

        return paths;
    }

    public static Polygon ToPoly(List<IntPoint> path)
    {
        Polygon poly = new(path.Count);
        foreach (var point in path)
        {
            poly.Add(new (point.X, point.Y));
        }

        return poly;
    }

    public static Polygons ToPolys(List<List<IntPoint>> paths)
    {
        Polygons polys = new Polygons(paths.Count);
        foreach (var path in paths)
        {
            polys.Add(ToPoly(path));
        }

        return polys;
    }
}