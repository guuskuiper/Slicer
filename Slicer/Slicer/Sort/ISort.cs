// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Sort
{
    public interface ISort
    {
        (SortedLayer, IntPoint) SortPolygons(Layer layer, IntPoint prevPt);
    }
}