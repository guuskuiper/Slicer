﻿using Slicer.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations.Triangulation
{
    public class DelaunayIncremental : ITriangulate
    {
        private List<Triangle> _triangles;
        private Rect _boundsOffset;
        private HashSet<Point2D> _exterior;
        private Dictionary<Point2D, Vertex> _mapping;
        private Triangle _base;
        
        /*
         * Start with 2 triangle covering the entire area.
         * Randomly insert a point from the polygons
         */
        public Polygons Triangulate(Polygons polygons)
        {
            
            _exterior = new HashSet<Point2D>();
            _mapping = new Dictionary<Point2D, Vertex>();

            long maxX = polygons.Max(x => x.Max(xx => xx.X));
            long minX = polygons.Min(x => x.Min(xx => xx.X));
            long maxY = polygons.Max(y => y.Max(yy => yy.Y));
            long minY = polygons.Min(y => y.Min(yy => yy.Y));
            Rect bounds = new (minX, maxY, maxX, minY);
            //IntRect bounds = ClipperBase.GetBounds(polygons.GetPolysCopy());
            _boundsOffset = new (bounds.left - 1, bounds.top + 1,bounds.right + 1, bounds.bottom - 1);
            foreach (Polygon polygon in polygons)
            {
                foreach (var point in polygon)
                {
                    _exterior.Add(point);
                }
            }

            InitializeBaseTriangle();

            foreach (Polygon polygon in polygons)
            {
                System.Random random = new System.Random(42);
                foreach (var point in polygon.OrderBy(r => random.Next()))
                {
                    InsertPoint(point);
                }
            }

            RemoveBaseTraingle();

            Polygons triangles = new Polygons();
            triangles.AddRange(_triangles.Select(x => new Polygon(x.P0, x.P1, x.P2)));
            
            return triangles;
        }

        private void InitializeBaseTriangle()
        {
            long height = _boundsOffset.top - _boundsOffset.bottom;
            long width = _boundsOffset.right - _boundsOffset.left;
            Point2D A = new (_boundsOffset.left, _boundsOffset.bottom - 10 * height);
            Point2D B = new (_boundsOffset.left, _boundsOffset.top);
            Point2D C = new (_boundsOffset.right + 10 * width, _boundsOffset.top);
            _base = new Triangle(A, B, C);
            _triangles = new List<Triangle> {_base};
        }
        
        private void RemoveBaseTraingle()
        {
            _triangles.RemoveAll(IsAtBaseBound);
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

        private bool IsAtBound(Triangle t)
        {
            bool isBound = false;

            isBound &= t.P0.X == _boundsOffset.left   || t.P1.X == _boundsOffset.left   || t.P2.X == _boundsOffset.left;
            isBound &= t.P0.X == _boundsOffset.right  || t.P1.X == _boundsOffset.right  || t.P2.X == _boundsOffset.right; 
            isBound &= t.P0.Y == _boundsOffset.top    || t.P1.Y == _boundsOffset.top    || t.P2.Y == _boundsOffset.top; 
            isBound &= t.P0.Y == _boundsOffset.bottom || t.P1.Y == _boundsOffset.bottom || t.P2.Y == _boundsOffset.bottom; 
            
            return isBound;
        }

        private void InsertPoint(Point2D pt)
        {
            foreach (Triangle triangle in _triangles)
            {
                if (PolyMath.PointInTriangle(pt, triangle))
                {
                    InsertTriangle(pt, triangle);
                    return;
                }

                //Returns 0 if false, -1 if pt is on poly and +1 if pt is in poly.
                // int pointIn = ClipperLib.Clipper.PointInPolygon(pt, new List<IntPoint> {triangle.P0, triangle.P1, triangle.P2});
                // if(pointIn > 0)
                // {
                //     InsertTriangle(pt, triangle);
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

        private void InsertTriangle(Point2D pt, Triangle t)
        {
            // insert pa, pb, pc
            _triangles.Add(new Triangle(t.P0, t.P1, pt));
            _triangles.Add(new Triangle(t.P1, t.P2, pt));
            _triangles.Add(new Triangle(t.P2, t.P0, pt));
            _triangles.Remove(t);

            SwapTest(t.P0, t.P1, pt);
            SwapTest(t.P1, t.P2, pt);
            SwapTest(t.P2, t.P0, pt);
        }

        private void SwapTest(Point2D a, Point2D b, Point2D p)
        {
            if(_base.Contains(a) && _base.Contains(b)) return;
            if(_exterior.Contains(a) && _exterior.Contains(b)) return;  // TODO check if 2 point are connected in original polygon

            Point2D d = FindOpposite(a, b, p);

            if (PolyMath.PointInCircle(d, new Triangle(b, p, a)))
            {
                FlipEdge(a, b, p, d);
                SwapTest(a, d, p);
                SwapTest(d, b, p);
            }
        }

        private void FlipEdge(Point2D a, Point2D b, Point2D p, Point2D d)
        {
            //var matches = _triangles.Where(t => t.Contains(a) && t.Contains(b));
            var count = _triangles.RemoveAll(t => t.Contains(a) && t.Contains(b));
            Debug.Assert(count == 2);
            
            _triangles.Add(new Triangle(a, p, d));
            _triangles.Add(new Triangle(b, d, p));
        }

        // using half-edge structure this will become very fast halfedge->twine->next->next.P0
        private Point2D FindOpposite(Point2D p0, Point2D p1, Point2D original)
        {
            Triangle match = _triangles.SingleOrDefault(t => t.Contains(p0) && t.Contains(p1) && !t.Contains(original));
            return match.GetOther(p0, p1);
        }
    }
}