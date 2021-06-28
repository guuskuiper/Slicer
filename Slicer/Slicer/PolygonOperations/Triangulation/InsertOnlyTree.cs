// unset

using ClipperLib;
using Slicer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations.Triangulation
{
    public class TriangleTree
    {
        public Triangle Triangle;

        public HalfEdge HalfEdge;

        public IntRect Bounds;
        public List<TriangleTree> Childs = new (3);

        public TriangleTree(Triangle t, HalfEdge he)
        {
            Triangle = t;
            HalfEdge = he;
            Bounds = new IntRect(
                Math.Min(Math.Min(t.P0.X, t.P1.X), t.P2.X),
                Math.Max(Math.Max(t.P0.Y, t.P1.Y), t.P2.Y),
                Math.Max(Math.Max(t.P0.X, t.P1.X), t.P2.X),
                Math.Min(Math.Min(t.P0.Y, t.P1.Y), t.P2.Y)
                );// l, t, r, b
        }
    }
    public class InsertOnlyTree : ITriangulate
    {
        private TriangleTree _root;
        private IntRect _boundsOffset;
        private HalfEdgeStructure _halfEdgeStructure;
        private HashSet<IntPoint> _exterior;
        private Triangle _base;

        public Polygons Triangulate(Polygons polygons)
        {
            SetExterior(polygons);
            var bounds = PolyMath.CalcBounds(polygons);
            _boundsOffset = new IntRect(bounds.left - 1, bounds.top + 1, bounds.right + 1, bounds.bottom - 1);

            InitializeBaseTriangle();
            
            foreach (Polygon polygon in polygons)
            {
                System.Random random = new System.Random(42);
                foreach (IntPoint point in polygon.OrderBy(r => random.Next()))
                {
                    InsertPoint(point);
                }
            }
            
            return new Polygons(GetResult());
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
            _root = new TriangleTree(new Triangle(A, B, C), _halfEdgeStructure.Edges.FirstOrDefault(e => e.Face != Face.OUTSIDE));
        }
        
        private IEnumerable<Polygon> GetResult()
        {
            DoSwaps();
            _halfEdgeStructure.RemoveOutside();
            return _halfEdgeStructure.Faces.Select(f => new Polygon(f.Outer.P0.P, f.Outer.Next.P0.P, f.Outer.Prev.P0.P));
        }
        
        private void InsertPoint(IntPoint pt)
        {
            var leaf = TraverseTree(_root, pt);

            InsertTriangle(pt, leaf);
        }

        private static bool PointInBounds(IntPoint pt, IntRect bounds)
        {
            return 
                pt.X > bounds.left && 
                pt.X < bounds.right &&
                pt.Y > bounds.bottom && 
                pt.Y < bounds.top;
        }

        private TriangleTree TraverseTree(TriangleTree current, IntPoint pt)
        {
            foreach (TriangleTree child in current.Childs)
            {
                if (PointInBounds(pt, child.Bounds) && PolyMath.PointInTriangle(pt, child.Triangle))
                {
                    return TraverseTree(child, pt);
                }
            }

            if (current.Childs.Count == 0)
            {
                return current;
            }
            
            throw new Exception($"Cannot find triangle to insert {pt}");
        }
        
        private void InsertTriangle(IntPoint pt, TriangleTree tree)
        {
            Vertex newVertex = _halfEdgeStructure.InsertInside(tree.HalfEdge, pt);

            foreach (var newHe in _halfEdgeStructure.GetVertexHalfEdges(newVertex))
            {
                tree.Childs.Add(new TriangleTree(new Triangle(newHe.P0.P, newHe.Next.P0.P, newHe.Prev.P0.P), newHe));
            }
        }
        
        private void SetExterior(Polygons polygons)
        {
            _exterior = new HashSet<IntPoint>();
            foreach (Polygon polygon in polygons)
            {
                foreach (IntPoint point in polygon)
                {
                    _exterior.Add(point);
                }
            }
        }

        private void DoSwaps()
        {
            return;
            var edges = _halfEdgeStructure.Edges.ToList();

            foreach (var edge in edges)
            {
                SwapTest(edge);
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
            if(PolyMath.PointInCCWCircle(vD.P, vB.P, vP.P, vA.P))
            {
                var newHalfEdge = _halfEdgeStructure.FlipEdge(e);

                SwapTest(newHalfEdge, depth+1);
                SwapTest(newHalfEdge.Twine, depth+1);
            }
        }
    }
}