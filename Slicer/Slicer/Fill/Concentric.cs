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

        public FillResult ConcentricFill(Polygon poly, double lineWidth, int maxPathCount) => 
            ConcentricFill(new Polygons(poly), lineWidth, maxPathCount);

        public FillResult ConcentricFill(Polygons polys, double lineWidth, int maxPathCount) =>
            ConcentricBase(polys, -lineWidth, maxPathCount);

        public FillResult ConcentricOutside(Polygons polygons, double lineWidth, int maxPathCount) =>
            ConcentricBase(polygons, lineWidth, maxPathCount);

        private FillResult ConcentricBase(Polygons polygons, double lineWidth, int maxPathCount)
        {
            Polygons paths = new();
            Polygons currentPolys = new();
            int pathCount = 0;
            
            currentPolys.AddRange(polygons);
            
            double currentOffset = lineWidth / 2;

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

            Polygons remainingPolys = _offset.PolyOffset(currentPolys, lineWidth / 2);

            return new FillResult(paths, remainingPolys);
        }
    }
}