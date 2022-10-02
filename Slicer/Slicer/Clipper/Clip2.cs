using Clipper2Lib;
using ClipperLib;
using Slicer.Models;
using System;
using ClipType = Clipper2Lib.ClipType;

namespace Slicer.Slicer.Clipper;

public class Clip2 : IClip
{
    private readonly Clipper64 _clipper = new ();
    public Polygons Difference(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, true);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Difference, FillRule.EvenOdd, solution);

        return ToPolys(solution);
    }

    public Polygons DifferenceOpen(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        Paths64 solutionOpen = new();
        _clipper.Clear();
        AddPolys(polys, false, false);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Difference, FillRule.EvenOdd, solution, solutionOpen);

        return ToPolys(solutionOpen);
    }

    public Polygons Union(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, true);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Union, FillRule.EvenOdd, solution);

        return ToPolys(TrimColinear(solution));
    }

    public (Polygons open, Polygons closed) UnionOpen(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        Paths64 solutionOpen = new();
        _clipper.Clear();
        AddPolys(polys, false, false);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Union, FillRule.EvenOdd, solution, solutionOpen);

        return (ToPolys(solutionOpen), ToPolys(solution));
    }

    public Polygons Intersection(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, true);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Intersection, FillRule.EvenOdd, solution);
        
        return ToPolys(solution);
    }

    public Polygons IntersectionOpen(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        Paths64 solutionOpen = new();
        _clipper.Clear();
        AddPolys(polys, false, false);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Intersection, FillRule.EvenOdd, solution, solutionOpen);

        return ToPolys(solutionOpen);
    }

    public Polygons Xor(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, polysIsClosed);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Xor, FillRule.EvenOdd, solution);

        return ToPolys(solution);
    }
    
    
    private void AddPolys(Polygons polys, bool isClip, bool isClosed)
    {
        foreach (var poly in polys)
        {
            var path = ToPath(poly);
            if (isClip)
            {
                _clipper.AddClip(path);
            }
            else
            {
                if (isClosed)
                    _clipper.AddSubject(path);
                else
                    _clipper.AddOpenSubject(path);
            }
        }
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

    private Paths64 TrimColinear(Paths64 input)
    {
        Paths64 paths = new Paths64(input.Count);
        foreach (Path64 path in input)
        {
            paths.Add(Clipper2Lib.Clipper.TrimCollinear(path));
        }

        return paths;
    }
}