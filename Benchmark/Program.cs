using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ClipperLib;
using Slicer.Slicer.Clipper;
using System.Collections.Generic;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
