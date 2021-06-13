// unset

using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Clipper
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

            return new Polygons(result);
        }

        public Polygons DifferenceOpen(Polygons polys, Polygons clipPolygons)
        {
            PolyTree result = new PolyTree();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, false);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctDifference, result);

            return new Polygons(ClipperLib.Clipper.OpenPathsFromPolyTree(result));
        }

        public Polygons Union(Polygons polys, Polygons clipPolygons)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, true);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctUnion, result);

            return new Polygons(result);
        }

        public (Polygons open, Polygons closed) UnionOpen(Polygons polys, Polygons clipPolygons)
        {
            PolyTree result = new PolyTree();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, false);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctUnion, result);

            Polygons open = new Polygons(ClipperLib.Clipper.OpenPathsFromPolyTree(result));
            Polygons closed = new Polygons(ClipperLib.Clipper.ClosedPathsFromPolyTree(result));

            return (open, closed);
        }

        public Polygons Intersection(Polygons polys, Polygons clipPolygons)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, true);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctIntersection, result);

            return new Polygons(result);
        }

        public Polygons IntersectionOpen(Polygons polys, Polygons clipPolygons)
        {
            PolyTree result = new PolyTree();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, false);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctIntersection, result);

            return new Polygons(ClipperLib.Clipper.OpenPathsFromPolyTree(result));
        }

        public Polygons Xor(Polygons polys, Polygons clipPolygons, bool polysIsClosed = true)
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();

            _clipper.Clear();
            AddPolys(polys, PolyType.ptSubject, polysIsClosed);
            AddPolys(clipPolygons, PolyType.ptClip, true);
            _clipper.Execute(ClipType.ctXor, result);

            return new Polygons(result);
        }

        private void AddPolys(Polygons polys, PolyType polyType, bool closed = true)
        {
            foreach (var poly in polys.Polys)
            {
                _clipper.AddPath(poly.Poly, polyType, closed);
            }
        }
    }
}