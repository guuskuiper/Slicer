// unset

using ClipperLib;
using Slicer.Models;

namespace Slicer.Slicer.Fill.Patterns
{
    public class ParallelPatternGenerator : IPatternGenerator
    {
        public const string Name = "Parallel";
        public string PatternName => Name;
        
        public IPattern CreatePattern(Project project, IntRect boundingBox)
        {
            var polygons = new Polygons();
            var lineWidth = project.Settings.LineWidth;

            var currentX = boundingBox.left + lineWidth / 2;

            while (currentX < boundingBox.right)
            {
                var polygon = new Polygon(
                    new IntPoint(currentX, boundingBox.bottom),
                    new IntPoint(currentX, boundingBox.top)
                );
                polygons.Add(polygon);

                currentX += lineWidth;
            }

            return new Pattern {Polygons = polygons, Name = PatternName};
        }
    }
}