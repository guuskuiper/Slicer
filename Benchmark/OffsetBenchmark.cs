using BenchmarkDotNet.Attributes;
using ClipperLib;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.Clipper.Clipper2;
using Slicer.Slicer.PolygonOperations;

namespace Benchmark
{
    [MemoryDiagnoser]
    public class OffsetBenchmark
    {
        private Polygon data = CreatePolygon.CreateCircle(new (0, 0), 10_000, 100);

        int NumberOfItems = 1000;
        private readonly IOffset _offset = new Offset(new ClipperOffset());
        private readonly IOffset _offsetCopy = new OffsetCopy();
        private readonly IOffset _offset2 = new Offset2();

        [Benchmark]
        public Polygons Offset()
        {
            Polygons result = new ();
            for (int i = 0; i < NumberOfItems; i++)
            {
                result = _offset.PolyOffset(data, 1000);
            }

            return result;
        }
        
        [Benchmark]
        public Polygons OffsetCopy()
        {
            Polygons result = new ();
            for (int i = 0; i < NumberOfItems; i++)
            {
                result = _offsetCopy.PolyOffset(data, 1000);
            }
            return result;
        }
        
        [Benchmark]
        public Polygons Offset2()
        {
            Polygons result = new ();
            for (int i = 0; i < NumberOfItems; i++)
            {
                result = _offset2.PolyOffset(data, 1000);
            }
            return result;
        }
    }
}
