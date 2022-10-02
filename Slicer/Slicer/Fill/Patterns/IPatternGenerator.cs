// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public interface IPatternGenerator
    {
        IPattern CreatePattern(Project project, Rect boundingBox);
        string PatternName { get;  }
    }
}