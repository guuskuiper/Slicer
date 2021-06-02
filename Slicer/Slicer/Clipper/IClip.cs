// unset

using Slicer.Models;

namespace Slicer.Slicer.Clipper
{
    public interface IClip
    {
        Polygons Difference(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true);
        Polygons Union(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true);
        Polygons Intersection(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true);
        Polygons Xor(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true);
    }
}