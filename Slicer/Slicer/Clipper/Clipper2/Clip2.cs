using Clipper2Lib;
using Slicer.Models;
using ClipType = Clipper2Lib.ClipType;

namespace Slicer.Slicer.Clipper.Clipper2;

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

        return Clipper2Converters.ToPolys(solution);
    }

    public Polygons DifferenceOpen(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        Paths64 solutionOpen = new();
        _clipper.Clear();
        AddPolys(polys, false, false);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Difference, FillRule.EvenOdd, solution, solutionOpen);

        return Clipper2Converters.ToPolys(solutionOpen);
    }

    public Polygons Union(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, true);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Union, FillRule.EvenOdd, solution);

        return Clipper2Converters.ToPolys(TrimColinear(solution));
    }

    public (Polygons open, Polygons closed) UnionOpen(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        Paths64 solutionOpen = new();
        _clipper.Clear();
        AddPolys(polys, false, false);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Union, FillRule.EvenOdd, solution, solutionOpen);

        return (Clipper2Converters.ToPolys(solutionOpen), Clipper2Converters.ToPolys(solution));
    }

    public Polygons Intersection(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, true);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Intersection, FillRule.EvenOdd, solution);
        
        return Clipper2Converters.ToPolys(solution);
    }

    public Polygons IntersectionOpen(Polygons polys, Polygons clipPolygons)
    {
        Paths64 solution = new();
        Paths64 solutionOpen = new();
        _clipper.Clear();
        AddPolys(polys, false, false);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Intersection, FillRule.EvenOdd, solution, solutionOpen);

        return Clipper2Converters.ToPolys(solutionOpen);
    }

    public Polygons Xor(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true)
    {
        Paths64 solution = new();
        _clipper.Clear();
        AddPolys(polys, false, polysIsClosed);
        AddPolys(clipPolygons, true, true);
        _clipper.Execute(ClipType.Xor, FillRule.EvenOdd, solution);

        return Clipper2Converters.ToPolys(solution);
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
        foreach (var point in poly)
        {
            path.Add(new Point64(point.X, point.Y));
        }

        return path;
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