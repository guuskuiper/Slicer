// unset

using ClipperLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Slicer.Slicer.PolygonOperations
{
    public class Vertex
    {
        public IntPoint P; // make generic? for IntPoint
        public HalfEdge E; // halfEdge that starts at this vertex
    }

    public class Face
    {
        public HalfEdge Outer;
        public List<HalfEdge> Inner = new List<HalfEdge>();

        public Face(HalfEdge outer)
        {
            this.Outer = outer;
        }
    }
    
    public class HalfEdge
    {
        public HalfEdge Twine;
        public Vertex P0;
        public Face Face; // can be null for outside
        public HalfEdge Next;
        public HalfEdge Prev;

        public HalfEdge(Vertex origin)
        {
            P0 = origin;
        }
    }

    public class HalfEdgeStructure
    {
        public List<Vertex> Vs;
        public List<HalfEdge> Es;
        public List<Face> Fs;

        public HalfEdgeStructure(IntPoint p0, IntPoint p1, IntPoint p2)
        {
            Vertex v0 = new Vertex() {P = p0};
            Vertex v1 = new Vertex() {P = p1};
            Vertex v2 = new Vertex() {P = p2};
            
            HalfEdge e01 = new HalfEdge(v0);
            HalfEdge e10 = new HalfEdge(v1);
            HalfEdge e12 = new HalfEdge(v1);
            HalfEdge e21 = new HalfEdge(v2);
            HalfEdge e20 = new HalfEdge(v2);
            HalfEdge e02 = new HalfEdge(v0);

            Face f = new Face(e01);
            
            e01.Next = e12;
            e01.Prev = e20;
            e01.Twine = e10;
            e01.Face = f;
            
            e12.Next = e20;
            e12.Prev = e01;
            e12.Twine = e21;
            e12.Face = f;
            
            e20.Next = e01;
            e20.Prev = e12;
            e20.Twine = e02;
            e20.Face = f;
            
            e10.Next = e02;
            e10.Prev = e21;
            e10.Twine = e01;
            e10.Face = null;
            
            e21.Next = e10;
            e21.Prev = e02;
            e21.Twine = e12;
            e21.Face = null;
            
            e02.Next = e21;
            e02.Prev = e10;
            e02.Twine = e20;
            e02.Face = null;

            v0.E = e01;
            v1.E = e12;
            v2.E = e20;

            Fs = new List<Face> {f};
            Vs = new List<Vertex> {v0, v1, v2};
            Es = new List<HalfEdge> { e01, e10, e12, e21, e20, e02 };
        }

        public void InsertOutside(HalfEdge h, IntPoint p)
        {
            Vertex v2 = new Vertex() {P = p};
            
            HalfEdge e12 = new HalfEdge(h.Next.P0);
            HalfEdge e21 = new HalfEdge(v2);
            HalfEdge e20 = new HalfEdge(v2);
            HalfEdge e02 = new HalfEdge(h.P0);
            
            Face f = new Face(e12);

            e12.Next = e20;
            e12.Prev = h;
            e12.Twine = e21;
            e12.Face = f;

            e20.Next = h;
            e20.Prev = e12;
            e20.Twine = e02;
            e20.Face = f;

            e02.Next = e21;
            e02.Prev = h.Prev;
            e02.Twine = e20;

            e21.Next = h.Next;
            e21.Prev = e02;
            e21.Twine = e12;

            h.Face = f;
            h.Next = e12;
            h.Prev = e20;
            h.Next.Prev = e21;
            h.Prev.Next = e02;

            v2.E = e20;
            
            Fs.Add(f);
            Vs.Add(v2);
            Es.AddRange(new List<HalfEdge> {e12, e21, e20, e02});
        }
        
        // p should be in the loop created from h
        public Vertex InsertInside(HalfEdge h, IntPoint p)
        {
            Vertex v = new Vertex() {P = p};

            var loop = GetLoop(h).ToList();
            List<HalfEdge> newHalfEdges = new List<HalfEdge>();
            List<Face> newFaces = new List<Face>();

            Fs.Remove(loop[0].Face);

            foreach (HalfEdge e in loop)
            {
                HalfEdge e2p = new HalfEdge(e.Next.P0);
                HalfEdge p2e = new HalfEdge(v);

                Face f = new Face(e2p);

                e2p.Next = p2e;
                e2p.Prev = e;
                e2p.Face = f;

                p2e.Next = e;
                p2e.Prev = e2p;
                p2e.Face = f;

                e.Next = e2p;
                e.Prev = p2e;
                e.Face = f;
                
                newFaces.Add(f);
                newHalfEdges.Add(e2p);
                newHalfEdges.Add(p2e);
            }

            for (int i = 0; i < loop.Count; i++)
            {
                HalfEdge e2p = newHalfEdges[i * 2];
                HalfEdge p2e = newHalfEdges[(i * 2 + 3) % newHalfEdges.Count];
                
                e2p.Twine = p2e;
                p2e.Twine = e2p;
            }

            v.E = newHalfEdges[1];
           
            Vs.Add(v);
            Fs.AddRange(newFaces);
            Es.AddRange(newHalfEdges);

            return v;
        }

        // assume all triangles ( GetLoop(e).Count == 3 )
        public HalfEdge FlipEdge(HalfEdge e)
        {
            Debug.Assert(GetLoop(e).Count() == 3);
            Debug.Assert(GetLoop(e.Twine).Count() == 3);

            var dPrev = e.Next;
            var dNext = e.Prev;
            var pPrev = e.Twine.Next;
            var pNext = e.Twine.Prev;
            var f0 = e.Face;
            var f1 = e.Twine.Face;
            
            RemoveEdge(e);

            var dp =  InsertEdge(dPrev, dNext, pPrev, pNext);

            UpdateFace(dp, f0);
            UpdateFace(dp.Twine, f1);
            
            return dp;
        }

        private void UpdateFace(HalfEdge e, Face f)
        {
            f.Outer = e;
            foreach (var l in GetLoop(e))
            {
                l.Face = f;
            }
        }

        private void RemoveEdge(HalfEdge e)
        {
            RemoveHalfEdge(e);
            RemoveHalfEdge(e.Twine);
        }

        private void RemoveHalfEdge(HalfEdge e)
        {
            HalfEdge t = e.Twine;
            
            e.Prev.Next = t.Next;
            e.Next.Prev = t.Prev;

            // check if any vertex references this halfEdge
            if (e.P0.E == e)
            {
                e.P0.E = t.Next;
            }
            
            Es.Remove(e);
        }

        public HalfEdge InsertEdge(HalfEdge dPrev, HalfEdge dNext, HalfEdge pPrev, HalfEdge pNext)
        {
            HalfEdge dp = new HalfEdge(dNext.P0);
            HalfEdge pd = new HalfEdge(pNext.P0);

            dp.Twine = pd;
            pd.Twine = dp;

            dPrev.Next = dp;
            dNext.Prev = pd;

            pPrev.Next = pd;
            pNext.Prev = dp;

            dp.Next = pNext;
            dp.Prev = dPrev;

            pd.Next = dNext;
            pd.Prev = pPrev;

            return dp;
        }

        public Vertex GetVertex(IntPoint p)
        {
            return Vs.SingleOrDefault(v => v.P == p);
        }

        public HalfEdge GetHalfEdgeBetween(Vertex A, Vertex B)
        {
            return GetVertexHalfEdges(A).SingleOrDefault(he => he.Next.P0 == B);
        }

        public IEnumerable<HalfEdge> GetLoop(HalfEdge e)
        {
            HalfEdge start = e;
            HalfEdge current = e;
            do
            {
                yield return current;
                current = current.Next;
            } while (current != start);
        }

        public IEnumerable<HalfEdge> GetVertexHalfEdges(Vertex v)
        {
            HalfEdge start = v.E;
            HalfEdge current = v.E;
            do
            {
                yield return current;
                current = current.Twine.Next;
            } while (current != start);
        }
    }
}