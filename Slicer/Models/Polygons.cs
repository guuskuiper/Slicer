using ClipperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slicer.Models
{
    public class Polygons : List<Polygon>
    {
        public Polygons()
        {
        }

        public Polygons(IEnumerable<Polygon> polys) : base(polys)
        {
        }

        public Polygons(params Polygon[] polys) : base(polys)
        {
        }

        public Polygons(int capacity) : base(capacity)
        {
        }

        public Polygons(IEnumerable<IEnumerable<IntPoint>> polys)
        {
            this.AddRange(polys.Select(x => new Polygon(x)));
        }

        public Polygons(List<List<IntPoint>> polys) : base(polys.Count)
        {
            this.AddRange(polys.Select(x => new Polygon(x)));
        }

        public List<List<IntPoint>> GetPolysCopy()
        {
            return this.Select(x => x.ToList()).ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Polygon polygon in this)
            {
                sb.AppendLine(polygon.ToString());
            }
            return sb.ToString();
        }

        public void Close()
        {
            foreach (Polygon poly in this)
            {
                poly.Close();
            }
        }
    }
}
