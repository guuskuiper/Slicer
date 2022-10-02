using System.Collections.Generic;
using System.Linq;

namespace Slicer.Models
{
    public class Polygon : List<Point2D>
    {
        public Polygon()
        {
        }

        public Polygon(IEnumerable<Point2D> poly) : base(poly)
        {
        }

        public Polygon(params Point2D[] poly) : base(poly)
        {
        }
        
        public Polygon(int capacity) : base(capacity)
        {
        }

        /* No copy! */
        public Polygon(List<Point2D> poly)
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
