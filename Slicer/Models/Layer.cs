// unset

namespace Slicer.Models
{
    public class Layer : SortedLayer
    {
        public Polygons Contour { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }
        public Polygons Paths { get; set; } = new ();

        public override string ToString()
        {
            return $"Layer: Height={Height:F3}";
        }
    }
}