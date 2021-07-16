// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public interface IPatternFactory
    {
        IPattern CreatePattern(Project project, IntRect boundingBox);
    }
}