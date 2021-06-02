// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill
{
    public interface IConcentric
    {
        Polygons ConcentricFill(Polygon poly, double lineWidth);
        Polygons ConcentricFill(Polygons polys, double lineWidth);
        Polygons ConcentricOutside(Polygons polygons, double lineWidth, int maxPathCount);
    }
}