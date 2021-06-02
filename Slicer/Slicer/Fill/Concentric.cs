using Slicer.Models;
using Slicer.Slicer.Clipper;

namespace Slicer.Slicer.Fill
{
    public class Concentric : IConcentric
    {
        private readonly IOffset _offset;

        public Concentric(IOffset offset)
        {
            _offset = offset;
        }

        public Polygons ConcentricFill(Polygon poly, double lineWidth)
        {
            return ConcentricFill(new Polygons(poly), lineWidth);
        }

        public Polygons ConcentricFill(Polygons polys, double lineWidth)
        {
            Polygons paths = new();

            double currentOffset = -lineWidth / 2;

            Polygons currentPolys = new();
            currentPolys.AddRange(polys);

            while (currentPolys.Count > 0)
            {
                currentPolys = _offset.PolyOffset(currentPolys, currentOffset);
                if (currentPolys.Count > 0)
                {
                    paths.AddRange(currentPolys);
                }

                currentOffset = -lineWidth;
            }

            paths.Close();

            return paths;
        }

        public Polygons ConcentricOutside(Polygons polygons, double lineWidth, int maxPathCount)
        {
            Polygons paths = new();
            int pathCount = 0;

            double currentOffset = lineWidth / 2;

            Polygons currentPolys = new();
            currentPolys.AddRange(polygons);

            while (currentPolys.Count > 0 && pathCount < maxPathCount)
            {
                currentPolys = _offset.PolyOffset(currentPolys, currentOffset);
                if (currentPolys.Count > 0)
                {
                    paths.AddRange(currentPolys);
                }

                pathCount++;
                currentOffset = lineWidth;
            }

            paths.Close();

            return paths;
        }
    }
}