using BenchmarkDotNet.Attributes;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.PolygonOperations;
using Slicer.Slicer.PolygonOperations.Triangulation;

namespace Benchmark
{
    [MemoryDiagnoser]
    public class TriangulationBenchmark
    {
        private Polygons data = new Polygons()
        {
            new Polygon {new(0, 0), new (10_000, 0), new (10_000, 10_000), new (0, 10_000)},
        };
        
        //IOffset offset = new OffsetCopy();

        private Polygons data2 = new OffsetCopy().PolyOffsetRound(new Polygon(CreatePolygon.SquarePoly(10)), 1_000_000);
        

        [Benchmark]
        public Polygons DelaunayIncremental()
        {
            ITriangulate delaunayIncremental = new DelaunayIncremental();
            return delaunayIncremental.Triangulate(data2);
        }
        
        [Benchmark]
        public Polygons DelaunayIncrementalHalfEdge()
        {
            ITriangulate delaunayIncrementalHalfEdge = new DelaunayIncrementalHalfEdge();
            return delaunayIncrementalHalfEdge.Triangulate(data2);
        }
        
        [Benchmark]
        public Polygons InsertOnly()
        {
            ITriangulate insertOnly = new InsertOnly();
            return insertOnly.Triangulate(data2);
        }
        
        [Benchmark]
        public Polygons InsertOnlyTree()
        {
            ITriangulate insertOnly = new InsertOnlyTree();
            return insertOnly.Triangulate(data2);
        }
    }
}