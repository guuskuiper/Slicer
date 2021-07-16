// unset

using Slicer.Models;
using Slicer.Slicer.Clipper;

namespace Slicer.Slicer.Fill.Patterns
{
    public class PatternFiller : IPatternFiller
    {
        private readonly IClip _clip;

        public PatternFiller(IClip clip)
        {
            _clip = clip;
        }

        public Polygons Fill(Polygons area, IPattern pattern)
        {
            return _clip.Intersection(pattern.Polygons, area);
        }
    }
}