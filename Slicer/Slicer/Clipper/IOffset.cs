// unset

using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Clipper
{
    public interface IOffset
    {
        Polygons PolyOffset(Polygon input, double offset);
        List<List<IntPoint>> PolyOffset(List<IntPoint> input, double offset);
        Polygons PolyOffset(Polygons input, double offset);
    }
}