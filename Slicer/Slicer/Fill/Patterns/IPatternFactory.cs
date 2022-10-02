// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public interface IPatternFactory
    {
        IPattern CreatePattern(Project project, Rect boundingBox);
    }
}