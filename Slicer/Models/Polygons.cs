using ClipperLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slicer.Models
{
    public class Polygons
    {
        public List<Polygon> Polys { get; set; }

        public int Count => Polys.Count;

        public Polygons()
        {
            Polys = new List<Polygon>();
        }

        public Polygons(IEnumerable<Polygon> polys)
        {
            Polys = new List<Polygon>(polys);
        }

        public Polygons(Polygon poly)
        {
            Polys = new List<Polygon> {poly};
        }

        public Polygons(int capacity)
        {
            Polys = new List<Polygon>(capacity);
        }

        public Polygons(IEnumerable<IEnumerable<IntPoint>> polys)
        {
            Polys = new List<Polygon>();
            foreach (IEnumerable<IntPoint> poly in polys)
            {
                Polys.Add(new Polygon(poly));
            }
        }

        public Polygons(List<List<IntPoint>> polys)
        {
            Polys = new List<Polygon>(polys.Count);
            foreach (List<IntPoint> poly in polys)
            {
                Polys.Add(new Polygon(poly));
            }
        }

        public void Add(Polygon poly)
        {
            Polys.Add(poly);
        }

        public void AddRange(Polygons polys)
        {
            Polys.AddRange(polys.Polys);
        }

        public List<List<IntPoint>> GetPolys()
        {
            return Polys.Select(x => x.Poly).ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Polygon polygon in Polys)
            {
                sb.AppendLine(polygon.ToString());
            }
            return sb.ToString();
        }

        public void Close()
        {
            foreach (Polygon poly in Polys)
            {
                poly.Close();
            }
        }
    }
}
