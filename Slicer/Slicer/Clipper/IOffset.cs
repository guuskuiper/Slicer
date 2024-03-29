﻿// unset

using Slicer.Models;

namespace Slicer.Slicer.Clipper
{
    public interface IOffset
    {
        Polygons PolyOffset(Polygon input, double offset);
        Polygons PolyOffsetRound(Polygon input, double offset);
        Polygons PolyOffset(Polygons input, double offset);
    }
}