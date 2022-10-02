// unset

using Slicer.Models;

namespace Slicer.Slicer.Sort
{
    public interface ISort
    {
        (Layer, Point2D) SortPolygons(Layer layer, Point2D prevPt);
        Point2D SortPolygonsInplace(Layer layer, Point2D prevPt);
    }
}