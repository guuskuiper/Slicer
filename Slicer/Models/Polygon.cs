using ClipperLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slicer.Models
{
    public class Polygon : List<IntPoint>
    {
        public Polygon()
        {
        }

        public Polygon(IEnumerable<IntPoint> poly) : base(poly)
        {
        }

        public Polygon(params IntPoint[] poly) : base(poly)
        {
        }

        /* No copy! */
        public Polygon(List<IntPoint> poly)
        {
            AddRange(poly);
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(p => $"({p.X},{p.Y})"));
        }

        public void Close()
        {
            if (this.Count < 3)
            {
                return;
            }

            if (this[^1] == this[0])
            {
                return;
            }

            this.Add(this[0]);
        }
    }
}
