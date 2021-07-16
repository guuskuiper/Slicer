// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public interface IPatternFiller
    {
        Polygons Fill(Polygons area, IPattern pattern);
    }
}