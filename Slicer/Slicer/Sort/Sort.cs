// unset

using Slicer.Models;

namespace Slicer.Slicer.Sort
{
    public class Sort : ISort
    {
        public (Layer, Point2D) SortPolygons(Layer layer, Point2D prevPt)
        {
            var sortedLayer = new Layer {Height = layer.Height, Thickness = layer.Thickness};

            Point2D curPoint = prevPt;
            foreach (var path in layer.Paths)
            {
                sortedLayer.Paths.Add(path);
                curPoint = path[^1];
            }

            return (sortedLayer, curPoint);
        }

        public Point2D SortPolygonsInplace(Layer layer, Point2D prevPt)
        {
            Polygons paths = layer.Paths;
            layer.Paths = new Polygons();
            
            Point2D curPoint = prevPt;
            foreach (var path in paths)
            {
                layer.Paths.Add(path);
                curPoint = path[^1];
            }

            return curPoint;
        }
    }
}