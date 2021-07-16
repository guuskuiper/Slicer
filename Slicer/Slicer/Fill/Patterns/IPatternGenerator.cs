// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public interface IPatternGenerator
    {
        IPattern CreatePattern(Project project, IntRect boundingBox);
        string PatternName { get;  }
    }
}