using ClipperLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slicer.Models
{
    public class Polygon : IEnumerable
    {
        public List<IntPoint> Poly { get; set; }

        public Polygon()
        {
            Poly = new List<IntPoint>();
        }

        public Polygon(IEnumerable<IntPoint> poly)
        {
            Poly = new List<IntPoint>(poly);
        }

        public Polygon(params IntPoint[] poly)
        {
            Poly = new List<IntPoint>(poly);
        }

        /* No copy! */
        public Polygon(List<IntPoint> poly)
        {
            Poly = poly;
        }

        public IEnumerator GetEnumerator()
        {
            yield return Poly.GetEnumerator();
        }

        public static implicit operator List<IntPoint> (Polygon polygon)
        {
            return polygon.Poly;
        }

        public override string ToString()
        {
            return string.Join(", ", Poly.Select(p => $"({p.X},{p.Y})"));
        }

        public void Close()
        {
            if (Poly.Count < 3)
            {
                return;
            }

            if (Poly[^1] == Poly[0])
            {
                return;
            }

            Poly.Add(Poly[0]);
        }
    }
}
