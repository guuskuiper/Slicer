// unset

using ClipperLib;
using Slicer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations.Triangulation
{
    public class InsertOnly : ITriangulate
    {
        private HalfEdgeStructure _halfEdgeStructure;
        private IntRect _boundsOffset;
        
        public Polygons Triangulate(Polygons polygons)
        {
            CalcBounds(polygons);
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

        private void CalcBounds(Polygons polygons)
        {
            long maxX = polygons.Max(x => x.Max(xx => xx.X));
            long minX = polygons.Min(x => x.Min(xx => xx.X));
            long maxY = polygons.Max(y => y.Max(yy => yy.Y));
            long minY = polygons.Min(y => y.Min(yy => yy.Y));
            IntRect bounds = new IntRect(minX, maxY, maxX, minY);
            _boundsOffset = new IntRect(bounds.left - 1, bounds.top + 1, bounds.right + 1, bounds.bottom - 1);
        }

        private void InitializeBaseTriangle()
        {
            long height = _boundsOffset.top - _boundsOffset.bottom;
            long width = _boundsOffset.right - _boundsOffset.left;
            IntPoint A = new IntPoint(_boundsOffset.left, _boundsOffset.bottom - 10 * height);
            IntPoint B = new IntPoint(_boundsOffset.left, _boundsOffset.top);
            IntPoint C = new IntPoint(_boundsOffset.right + 10 * width, _boundsOffset.top);
            
            _halfEdgeStructure = new HalfEdgeStructure(A, B, C);
        }

        private IEnumerable<Polygon> GetResult()
        {
            _halfEdgeStructure.RemoveOutside();
            return _halfEdgeStructure.Faces.Select(f => new Polygon(f.Outer.P0.P, f.Outer.Next.P0.P, f.Outer.Prev.P0.P));
        }

        private void InsertPoint(IntPoint pt)
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
                // int pointIn = ClipperLib.Clipper.PointInPolygon(pt, new List<IntPoint> {triangle.P0, triangle.P1, triangle.P2});
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
        
        private void InsertTriangle(IntPoint pt, HalfEdge he)
        {
            _ = _halfEdgeStructure.InsertInside(he, pt);
        }
    }
}