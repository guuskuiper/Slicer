using Clipper2Lib;
using Slicer.Models;

namespace Slicer.Slicer.Clipper.Clipper2;

public static class Clipper2Converters
{
    
    public static Path64 ToPath(Polygon poly)
    {
        Path64 path = new(poly.Count);
        foreach (var point in poly)
        {
            path.Add(new Point64(point.X, point.Y));
        }

        return path;
    }
    
    public static Paths64 ToPaths(Polygons polys)
    {
        Paths64 paths = new(polys.Count);
        foreach (var poly in polys)
        {
            paths.Add(ToPath(poly));
        }

        return paths;
    }

    public static Polygon ToPoly(Path64 path)
    {
        Polygon poly = new(path.Count);
        foreach (Point64 point in path)
        {
            poly.Add(new (point.X, point.Y));
        }

        return poly;
    }

    public static Polygons ToPolys(Paths64 paths)
    {
        Polygons polys = new Polygons(paths.Count);
        foreach (Path64 path in paths)
        {
            polys.Add(ToPoly(path));
        }

        return polys;
    }
}