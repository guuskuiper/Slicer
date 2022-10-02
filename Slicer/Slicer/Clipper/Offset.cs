using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Slicer.Slicer.Clipper
{

    public class Offset : IOffset
    {
        private readonly ClipperOffset _clipperOffset;

        public Offset(ClipperOffset clipperOffset)
        {
            _clipperOffset = clipperOffset;
        }

        public Polygons PolyOffset(Polygon input, double offset)
        {
            _clipperOffset.Clear();
            List<List<IntPoint>> result = new();
            _clipperOffset.AddPath(input, JoinType.jtMiter, EndType.etClosedPolygon);
            _clipperOffset.Execute(ref result, offset);

            return new Polygons(result);
        }

        public Polygons PolyOffset(Polygons input, double offset)
        {
            _clipperOffset.Clear();
            List<List<IntPoint>> result = new();
            AddPolys(input);
            _clipperOffset.Execute(ref result, offset);

            return new Polygons(result);
        }
        
        public Polygons PolyOffsetRound(Polygon input, double offset)
        {
            _clipperOffset.Clear();
            List<List<IntPoint>> result = new();
            _clipperOffset.AddPath(input, JoinType.jtRound, EndType.etClosedPolygon);
            _clipperOffset.Execute(ref result, offset);

            return new Polygons(result);
        }

        private void AddPolys(Polygons input, JoinType jt = JoinType.jtMiter)
        {
            foreach (Polygon poly in input)
            {
                _clipperOffset.AddPath(poly, jt, EndType.etClosedPolygon);
            }
        }
    }
}
