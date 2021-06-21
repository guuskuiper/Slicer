// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Sort
{
    public class Sort : ISort
    {
        public (Layer, IntPoint) SortPolygons(Layer layer, IntPoint prevPt)
        {
            var sortedLayer = new Layer {Height = layer.Height, Thickness = layer.Thickness};

            IntPoint curPoint = prevPt;
            foreach (var path in layer.Paths)
            {
                sortedLayer.Paths.Add(path);
                curPoint = path[^1];
            }

            return (sortedLayer, curPoint);
        }

        public IntPoint SortPolygonsInplace(Layer layer, IntPoint prevPt)
        {
            Polygons paths = layer.Paths;
            layer.Paths = new Polygons();
            
            IntPoint curPoint = prevPt;
            foreach (var path in layer.Paths)
            {
                layer.Paths.Add(path);
                curPoint = path[^1];
            }

            return curPoint;
        }
    }
}