using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Clipper.Clipper1
{

    public class OffsetCopy : IOffset
    {
        public Polygons PolyOffset(Polygon input, double offset)
        {
            var clipperOffset = new ClipperOffset();
            clipperOffset.Clear();
            List<List<IntPoint>> result = new();
            clipperOffset.AddPath(Clipper1Converters.ToPath(input), JoinType.jtMiter, EndType.etClosedPolygon);
            clipperOffset.Execute(ref result, offset);

            return Clipper1Converters.ToPolys(result);
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

            return Clipper1Converters.ToPolys(result);
        }
        
        public Polygons PolyOffsetRound(Polygon input, double offset)
        {
            var clipperOffset = new ClipperOffset();
            List<List<IntPoint>> result = new();
            clipperOffset.AddPath(Clipper1Converters.ToPath(input), JoinType.jtRound, EndType.etClosedPolygon);
            clipperOffset.Execute(ref result, offset);

            return Clipper1Converters.ToPolys(result);
        }

        private void AddPolys(ClipperOffset clipperOffset, Polygons input, JoinType jt = JoinType.jtMiter)
        {
            foreach (Polygon poly in input)
            {
                clipperOffset.AddPath(Clipper1Converters.ToPath(poly), jt, EndType.etClosedPolygon);
            }
        }
    }
}
