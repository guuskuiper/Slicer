// unset

using ClipperLib;
using Slicer.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations
{
    public class DelaunayIncrementalHalfEdge : ITriangulate
    {
        private HalfEdgeStructure _halfEdgeStructure;
        private IntRect _boundsOffset;
        private HashSet<IntPoint> _exterior;
        private Triangle _base;
        
        public Polygons Triangulate(Polygons polygons)
        {
            _exterior = new HashSet<IntPoint>();

            long maxX = polygons.Max(x => x.Max(xx => xx.X));
            long minX = polygons.Min(x => x.Min(xx => xx.X));
            long maxY = polygons.Max(y => y.Max(yy => yy.Y));
            long minY = polygons.Min(y => y.Min(yy => yy.Y));
            IntRect bounds = new IntRect(minX, maxY, maxX, minY);
            _boundsOffset = new IntRect(bounds.left - 1, bounds.top + 1,bounds.right + 1, bounds.bottom - 1);
            foreach (Polygon polygon in polygons)
            {
                foreach (IntPoint point in polygon)
                {
                    _exterior.Add(point);
                }
            }

            InitializeBaseTriangle();
            
            foreach (Polygon polygon in polygons)
            {
                System.Random random = new System.Random(42);
                foreach (IntPoint point in polygon.OrderBy(r => random.Next()))
                {
                    InsertPoint(point);
                }
            }
            
            var traingles = RemoveBaseTraingle();
            
            Polygons polys = new Polygons();
            polys.AddRange(traingles.Select(x => new Polygon(x.P0, x.P1, x.P2)));

            return polys;
        }
        
        private void InitializeBaseTriangle()
        {
            long height = _boundsOffset.top - _boundsOffset.bottom;
            long width = _boundsOffset.right - _boundsOffset.left;
            IntPoint A = new IntPoint(_boundsOffset.left, _boundsOffset.bottom - 10 * height);
            IntPoint B = new IntPoint(_boundsOffset.left, _boundsOffset.top);
            IntPoint C = new IntPoint(_boundsOffset.right + 10 * width, _boundsOffset.top);
            _base = new Triangle(A, B, C);
            
            _halfEdgeStructure = new HalfEdgeStructure(A, B, C);
            _ = _halfEdgeStructure.GetHalfEdgeBetween(_halfEdgeStructure.GetVertex(A), _halfEdgeStructure.GetVertex(C));
        }
        
        private List<Triangle> RemoveBaseTraingle()
        {
            var triangles =  _halfEdgeStructure.Fs.Select(x => new Triangle(x.Outer.P0.P, x.Outer.Next.P0.P, x.Outer.Prev.P0.P)).ToList();
            triangles.RemoveAll(IsAtBaseBound);
            return triangles;
        }
        
        private bool IsAtBaseBound(Triangle t)
        {
            bool isBound = false;

            isBound |= t.P0.X == _base.P0.X || t.P1.X == _base.P0.X || t.P2.X == _base.P0.X;
            isBound |= t.P0.X == _base.P2.X || t.P1.X == _base.P2.X || t.P2.X == _base.P2.X;
            isBound |= t.P0.Y == _base.P0.Y || t.P1.Y == _base.P0.Y || t.P2.Y == _base.P0.Y; 
            isBound |= t.P0.Y == _base.P2.Y || t.P1.Y == _base.P2.Y || t.P2.Y == _base.P2.Y; 
            
            return isBound;
        }
        
        private void InsertPoint(IntPoint pt)
        {
            foreach (Face face in _halfEdgeStructure.Fs)
            {
                var e = face.Outer;
                var triangle = new Triangle(e.P0.P, e.Next.P0.P, e.Prev.P0.P);
                if (PolyMath.PointInTriangle(pt, triangle))
                {
                    InsertTriangle(pt, e);
                    return;
                }
                
                //Returns 0 if false, -1 if pt is on poly and +1 if pt is in poly.
                int pointIn = ClipperLib.Clipper.PointInPolygon(pt, new List<IntPoint> {triangle.P0, triangle.P1, triangle.P2});
                if(pointIn > 0)
                {
                    InsertTriangle(pt, e);
                    return;
                }
                
                // point on polygon
                if (pointIn < 0)
                {
                    return;   
                }
            }

            // Point ON a triangle? what to do? (nothing?)
            Debug.Fail("Point not in a triangle");
        }
        
        private void InsertTriangle(IntPoint pt, HalfEdge he)
        {
            // insert pa, pb, pc
            List<HalfEdge> oldTriangle = _halfEdgeStructure.GetLoop(he).ToList();
            Vertex vPt = _halfEdgeStructure.InsertInside(he, pt);

            // TODO disabled for now
            Debug.Assert(oldTriangle.Count == 3);
            foreach (HalfEdge oldHe in oldTriangle)
            {
                SwapTest(oldHe);
            }
        }
        
        private void SwapTest(HalfEdge e)
        {
            if(_base.Contains(e.P0.P) && _base.Contains(e.Next.P0.P)) return;
            if(_exterior.Contains(e.P0.P) && _exterior.Contains(e.Next.P0.P)) return;

            var vD = e.Twine.Prev.P0;
            var vA = e.P0;
            var vB = e.Next.P0;
            var vP = e.Prev.P0;
            if (PolyMath.PointInCircle(vD.P, new Triangle(vB.P, vP.P, vA.P)))
            {
                var newHalfEdge = _halfEdgeStructure.FlipEdge(e);

                SwapTest(newHalfEdge);
                SwapTest(newHalfEdge.Twine);
            }
        }
    }
}