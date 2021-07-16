// unset

using Slicer.Slicer.Fill.Patterns;

namespace Slicer.Settings
{
    public class Settings
    {
        public double LineWidth { get; set; } = 1000;
        public double PrintSpeed { get; set; } = 10;
        public double TravelSpeed { get; set; } = 20;
        public int BrimCount { get; set; } = 1;
        public float LayerHeight { get; set; } = 0.2f;
        public int ConcentricPathCount { get; set; } = 1;
        public string PatternName { get; set; } = ParallelPatternGenerator.Name;
    }
}