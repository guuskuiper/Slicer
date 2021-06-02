using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Clipper
{

    public class OffsetCopy : IOffset
    {
        public Polygons PolyOffset(Polygon input, double offset)
        {
            var clipperOffset = new ClipperOffset();
            clipperOffset.Clear();
            List<List<IntPoint>> result = new();
            clipperOffset.AddPath(input, JoinType.jtMiter, EndType.etClosedPolygon);
            clipperOffset.Execute(ref result, offset);

            return new Polygons(result);
        }
        public List<List<IntPoint>> PolyOffset(List<IntPoint> input, double offset)
        {
            var clipperOffset = new ClipperOffset();
            clipperOffset.Clear();
            List<List<IntPoint>> result = new();
            clipperOffset.AddPath(input, JoinType.jtMiter, EndType.etClosedPolygon);
            clipperOffset.Execute(ref result, offset);

            return result;
        }

        public Polygons PolyOffset(Polygons input, double offset)
        {
            var clipperOffset = new ClipperOffset();
            List<List<IntPoint>> result = new();
            AddPolys(clipperOffset, input);
            clipperOffset.Execute(ref result, offset);

            return new Polygons(result);
        }

        private void AddPolys(ClipperOffset clipperOffset, Polygons input)
        {
            foreach (Polygon poly in input.Polys)
            {
                clipperOffset.AddPath(poly.Poly, JoinType.jtMiter, EndType.etClosedPolygon);
            }
        }
    }
}
