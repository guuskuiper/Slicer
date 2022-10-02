using Slicer.Models;
using EndType = Clipper2Lib.EndType;
using JoinType = Clipper2Lib.JoinType;

namespace Slicer.Slicer.Clipper.Clipper2;

public class Offset2 : IOffset
{
    private readonly Clipper2Lib.ClipperOffset _clipperOffset = new();
    public Polygons PolyOffset(Polygon input, double offset)
    {
        _clipperOffset.Clear();
        _clipperOffset.AddPath(Clipper2Converters.ToPath(input), JoinType.Miter, EndType.Polygon);
        var result = _clipperOffset.Execute(offset);

        return Clipper2Converters.ToPolys(result);
    }

    public Polygons PolyOffsetRound(Polygon input, double offset)
    {
        _clipperOffset.Clear();
        _clipperOffset.AddPath(Clipper2Converters.ToPath(input), JoinType.Round, EndType.Polygon);
        var result = _clipperOffset.Execute(offset);

        return Clipper2Converters.ToPolys(result);
    }

    public Polygons PolyOffset(Polygons input, double offset)
    {
        _clipperOffset.Clear();
        AddPolys(input);
        var result = _clipperOffset.Execute(offset);

        return Clipper2Converters.ToPolys(result);
    }
    
    private void AddPolys(Polygons input, JoinType jt = JoinType.Miter)
    {
        var paths = Clipper2Converters.ToPaths(input);
        _clipperOffset.AddPaths(paths, jt, EndType.Polygon);
    }
}