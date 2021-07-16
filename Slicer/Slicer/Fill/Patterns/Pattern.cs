// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public class Pattern : IPattern
    {
        public Polygons Polygons { get; set; }
        public string Name { get; set; }
    }
}