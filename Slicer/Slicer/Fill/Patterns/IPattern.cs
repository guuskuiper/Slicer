// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public interface IPattern
    {
        Polygons Polygons { get; }
        string Name { get; }
    }
}