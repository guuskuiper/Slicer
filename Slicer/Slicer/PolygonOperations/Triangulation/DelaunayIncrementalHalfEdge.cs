// unset

using Slicer.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations.Triangulation
{
    public class DelaunayIncrementalHalfEdge : ITriangulate
    {
        private HalfEdgeStructure _halfEdgeStructure;
        private Rect _boundsOffset;
        private HashSet<Point2D> _exterior;
        private Triangle _base;
        
        public Polygons Triangulate(Polygons polygons)
        {
            CalcBounds(polygons);
            SetExterior(polygons);
            InitializeBaseTriangle();
            
            foreach (Polygon polygon in polygons)
            {
                System.Random random = new System.Random(42);
                foreach (var point in polygon.OrderBy(r => random.Next()))
                {
                    InsertPoint(point);
                }
            }

            return new Polygons(GetResult());
        }

        private void SetExterior(Polygons polygons)
        {
            _exterior = new HashSet<Point2D>();
            foreach (Polygon polygon in polygons)
            {
                foreach (var point in polygon)
                {
                    _exterior.Add(point);
                }
            }
        }

        private void CalcBounds(Polygons polygons)
        {
            long maxX = polygons.Max(x => x.Max(xx => xx.X));
            long minX = polygons.Min(x => x.Min(xx => xx.X));
            long maxY = polygons.Max(y => y.Max(yy => yy.Y));
            long minY = polygons.Min(y => y.Min(yy => yy.Y));
            Rect bounds = new (minX, maxY, maxX, minY);
            _boundsOffset = new (bounds.left - 1, bounds.top + 1, bounds.right + 1, bounds.bottom - 1);
        }

        private void InitializeBaseTriangle()
        {
            long height = _boundsOffset.top - _boundsOffset.bottom;
            long width = _boundsOffset.right - _boundsOffset.left;
            Point2D A = new (_boundsOffset.left, _boundsOffset.bottom - 10 * height);
            Point2D B = new (_boundsOffset.left, _boundsOffset.top);
            Point2D C = new (_boundsOffset.right + 10 * width, _boundsOffset.top);
            _base = new Triangle(A, B, C);
            
            _halfEdgeStructure = new HalfEdgeStructure(A, B, C);
        }

        private IEnumerable<Polygon> GetResult()
        {
            _halfEdgeStructure.RemoveOutside();
            return _halfEdgeStructure.Faces.Select(f => new Polygon(f.Outer.P0.P, f.Outer.Next.P0.P, f.Outer.Prev.P0.P));
        }

        private void InsertPoint(Point2D pt)
        {
            // TODO: very slow
            foreach (Face face in _halfEdgeStructure.Faces)
            {
                var e = face.Outer;
                if (PolyMath.PointInTriangle(pt, e.P0.P, e.Next.P0.P, e.Prev.P0.P))
                {
                    InsertTriangle(pt, e);
                    return;
                }
                
                // //Returns 0 if false, -1 if pt is on poly and +1 if pt is in poly.
                // int pointIn = ClipperLib.Clipper.PointInPolygon(pt, new List<IntPoint> {e.P0.P, e.Next.P0.P, e.Prev.P0.P});
                // if(pointIn > 0)
                // {
                //     InsertTriangle(pt, e);
                //     return;
                // }
                //
                // // point on polygon
                // if (pointIn < 0)
                // {
                //     return;   
                // }
            }

            // Point ON a triangle? what to do? (nothing?)
            Debug.Fail("Point not in a triangle");
        }
        
        private void InsertTriangle(Point2D pt, HalfEdge he)
        {
            // insert pa, pb, pc
            List<HalfEdge> oldTriangle = _halfEdgeStructure.GetLoop(he).ToList();
            _ = _halfEdgeStructure.InsertInside(he, pt);
            
            //return; // skip swaps for now

            Debug.Assert(oldTriangle.Count == 3);
            foreach (HalfEdge oldHe in oldTriangle)
            {
                SwapTest(oldHe);
            }
        }

        private void SwapTest(HalfEdge e, int depth = 0)
        {
            if(_base.Contains(e.P0.P) && _base.Contains(e.Next.P0.P)) return;
            if(_exterior.Contains(e.P0.P) && _exterior.Contains(e.Next.P0.P)) return; // TODO check if 2 point are connected in original polygon

            Debug.WriteLine($"SwapTest depth={depth.ToString()}");
            
            // var vD = e.Twine.Prev.P0;
            // var vA = e.P0;
            // var vB = e.Next.P0;
            // var vP = e.Prev.P0;
            var vD = e.Prev.P0;
            var vA = e.P0;
            var vB = e.Next.P0;
            var vP = e.Twine.Prev.P0;
            //if (PolyMath.PointInCircle(vD.P, new Triangle(vB.P, vP.P, vA.P)))
            if(PolyMath.PointInCCWCircle(vD.P, vB.P, vP.P, vA.P))
            {
                var newHalfEdge = _halfEdgeStructure.FlipEdge(e);

                SwapTest(newHalfEdge, depth+1);
                SwapTest(newHalfEdge.Twine, depth+1);
            }
        }
        
        public Vertex GetVertex(Point2D p)
        {
            return _halfEdgeStructure.Vertices.SingleOrDefault(v => v.P == p);
        }

        public HalfEdge GetHalfEdgeBetween(Vertex A, Vertex B)
        {
            return _halfEdgeStructure.GetVertexHalfEdges(A).SingleOrDefault(he => he.Next.P0 == B);
        }
    }
}