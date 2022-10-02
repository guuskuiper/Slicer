// unset

using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Clipper.Clipper1
{
    public class Clip : IClip
    {
        private readonly ClipperLib.Clipper _clipper;

        public Clip(ClipperLib.Clipper clipper)
        {
            _clipper = clipper;
        }

        public Polygons Difference(Polygons polys, Polygons clipPolygons)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, true);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctDifference, result);

            return Clipper1Converters.ToPolys(result);
        }

        public Polygons DifferenceOpen(Polygons polys, Polygons clipPolygons)
        {
            PolyTree result = new PolyTree();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, false);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctDifference, result);

            return Clipper1Converters.ToPolys(ClipperLib.Clipper.OpenPathsFromPolyTree(result));
        }

        public Polygons Union(Polygons polys, Polygons clipPolygons)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, true);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctUnion, result);

            return Clipper1Converters.ToPolys(result);
        }

        public (Polygons open, Polygons closed) UnionOpen(Polygons polys, Polygons clipPolygons)
        {
            PolyTree result = new PolyTree();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, false);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctUnion, result);

            Polygons open = Clipper1Converters.ToPolys(ClipperLib.Clipper.OpenPathsFromPolyTree(result));
            Polygons closed = Clipper1Converters.ToPolys(ClipperLib.Clipper.ClosedPathsFromPolyTree(result));

            return (open, closed);
        }

        public Polygons Intersection(Polygons polys, Polygons clipPolygons)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, true);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctIntersection, result);

            return Clipper1Converters.ToPolys(result);
        }

        public Polygons IntersectionOpen(Polygons polys, Polygons clipPolygons)
        {
            PolyTree result = new PolyTree();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, false);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctIntersection, result);

            return Clipper1Converters.ToPolys(ClipperLib.Clipper.OpenPathsFromPolyTree(result));
        }

        public Polygons Xor(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, polysIsClosed);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctXor, result);

            return Clipper1Converters.ToPolys(result);
        }

        private void AddPolys(Polygons polys, PolyType polyType, bool closed = true)
        {
            foreach (var poly in polys)
            {
                _clipper.AddPath(Clipper1Converters.ToPath(poly), polyType, closed);
            }
        }
    }
}