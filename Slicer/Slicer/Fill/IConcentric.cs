// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill
{
    public interface IConcentric
    {
        FillResult ConcentricFill(Polygon poly, double lineWidth, int maxPathCount);
        FillResult ConcentricFill(Polygons polys, double lineWidth, int maxPathCount);
        FillResult ConcentricOutside(Polygons polygons, double lineWidth, int maxPathCount);
    }
}