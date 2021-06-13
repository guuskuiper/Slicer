// unset

using Slicer.Models;

namespace Slicer.Slicer.Clipper
{
    public interface IClip
    {
        Polygons Difference(Polygons polys, Polygons clipPolygons);
        Polygons DifferenceOpen(Polygons polys, Polygons clipPolygons);
        Polygons Union(Polygons polys, Polygons clipPolygons);
        (Polygons open, Polygons closed) UnionOpen(Polygons polys, Polygons clipPolygons);
        Polygons Intersection(Polygons polys, Polygons clipPolygonse);
        Polygons IntersectionOpen(Polygons polys, Polygons clipPolygons);
        Polygons Xor(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true);
    }
}