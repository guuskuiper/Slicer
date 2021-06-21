// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Sort
{
    public interface ISort
    {
        (Layer, IntPoint) SortPolygons(Layer layer, IntPoint prevPt);
        IntPoint SortPolygonsInplace(Layer layer, IntPoint prevPt);
    }
}