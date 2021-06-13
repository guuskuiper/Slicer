// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Sort
{
    public class Sort : ISort
    {
        public (SortedLayer, IntPoint) SortPolygons(Layer layer, IntPoint prevPt)
        {
            var sortedLayer = new SortedLayer {Height = layer.Height, Thickness = layer.Thickness};

            IntPoint curPoint = prevPt;
            foreach (var path in layer.Paths)
            {
                sortedLayer.Paths.Add(path);
                curPoint = path[^1];
            }

            return (sortedLayer, curPoint);
        }
    }
}