// unset

namespace Slicer.Models
{
    public class SortedLayer
    {
        public double Height { get; set; }
        public double Thickness { get; set; }
        public Polygons Paths { get; set; } = new();

        public override string ToString()
        {
            return $"Layer: Height={Height:F3}";
        }
    }
}