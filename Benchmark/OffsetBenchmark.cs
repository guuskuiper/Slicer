using BenchmarkDotNet.Attributes;
using ClipperLib;
using Slicer.Slicer.Clipper;
using System;
using System.Collections.Generic;

namespace Benchmark
{
    [MemoryDiagnoser]
    public class OffsetBenchmark
    {
        private List<IntPoint> data = new List<IntPoint>()
        {
            new(0, 0), new (10_000, 0), new (10_000, 10_000), new (0, 10_000),
        };

        int NumberOfItems = 100000;
        private readonly IOffset _offset = new Offset(new ClipperOffset());
        private readonly IOffset _offsetCopy = new OffsetCopy();

        [Benchmark]
        public List<List<IntPoint>> Offset()
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                result = _offset.PolyOffset(data, 1000);
            }

            return result;
        }
        [Benchmark]
        public List<List<IntPoint>> OffsetCopy()
        {
            List<List<IntPoint>> result = new List<List<IntPoint>>();
            for (int i = 0; i < NumberOfItems; i++)
            {
                result = _offsetCopy.PolyOffset(data, 1000);
            }
            return result;
        }
    }
}
