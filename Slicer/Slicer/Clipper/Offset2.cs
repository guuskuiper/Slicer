using Clipper2Lib;
using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;
using EndType = Clipper2Lib.EndType;
using JoinType = Clipper2Lib.JoinType;

namespace Slicer.Slicer.Clipper;

public class Offset2 : IOffset
{
    private readonly Clipper2Lib.ClipperOffset _clipperOffset = new();
    public Polygons PolyOffset(Polygon input, double offset)
    {
        _clipperOffset.Clear();
        _clipperOffset.AddPath(ToPath(input), JoinType.Miter, EndType.Polygon);
        var result = _clipperOffset.Execute(offset);

        return ToPolys(result);
    }

    public Polygons PolyOffsetRound(Polygon input, double offset)
    {
        _clipperOffset.Clear();
        _clipperOffset.AddPath(ToPath(input), JoinType.Round, EndType.Polygon);
        var result = _clipperOffset.Execute(offset);

        return ToPolys(result);
    }

    public Polygons PolyOffset(Polygons input, double offset)
    {
        _clipperOffset.Clear();
        AddPolys(input);
        var result = _clipperOffset.Execute(offset);

        return ToPolys(result);
    }
    
    private void AddPolys(Polygons input, JoinType jt = JoinType.Miter)
    {
        var paths = ToPaths(input);
        _clipperOffset.AddPaths(paths, jt, EndType.Polygon);
    }
    
    private Path64 ToPath(Polygon poly)
    {
        Path64 path = new(poly.Count);
        foreach (IntPoint point in poly)
        {
            path.Add(new Point64(point.X, point.Y));
        }

        return path;
    }
    
    private Paths64 ToPaths(Polygons polys)
    {
        Paths64 paths = new(polys.Count);
        foreach (var poly in polys)
        {
            paths.Add(ToPath(poly));
        }

        return paths;
    }

    private Polygon ToPoly(Path64 path)
    {
        Polygon poly = new(path.Count);
        foreach (Point64 point in path)
        {
            poly.Add(new IntPoint(point.X, point.Y));
        }

        return poly;
    }

    private Polygons ToPolys(Paths64 paths)
    {
        Polygons polys = new Polygons(paths.Count);
        foreach (Path64 path in paths)
        {
            polys.Add(ToPoly(path));
        }

        return polys;
    }
}