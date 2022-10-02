// unset

using Slicer.Models;
using Slicer.Slicer.PolygonOperations;
using System;
using System.Collections.Generic;

namespace Slicer.Slicer.Slice
{
    public class SliceLayer : ISliceLayer
    {
        private readonly Intersection _intersection;
        private readonly byte[] _data;

        public SliceLayer(Intersection intersection)
        {
            _intersection = intersection;
            _data = new byte[1_000_000];
        }

        public void CreateLayerContour(Layer layer, STL stl)
        {
            float z = (float)(layer.Height - layer.Thickness / 2);

            var lines = _intersection.Intersections(z, stl.Triangles);

            layer.Contour = ConnectLines(lines);
        }

        private Polygons ConnectLines(List<Line> lines)
        {
            //var linesCopy = new List<Line>(lines);
            var polys = new Polygons();

            while (lines.Count > 0)
            {
                var line = lines[0];
                lines.RemoveAt(0);
                var poly = new Polygon(line.Pt0, line.Pt1);

                ConnectLine(lines, poly);

                polys.Add(poly);
            }

            polys.RemoveAll(x => x.Count < 3);

            polys.Close();

            return Simplify.Reduce(polys, 10); ;
        }

        private void ConnectLine(List<Line> lines, Polygon poly)
        {
            bool progress = true;
            while (progress)
            {
                progress = false;
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    var curLine = lines[i];
                    if (AlmostEqual(poly[^1], curLine.Pt0))
                    {
                        poly.Add(curLine.Pt1);
                        lines.RemoveAt(i);
                        progress = true;
                        break;
                    }

                    if (AlmostEqual(poly[^1], curLine.Pt1))
                    {
                        poly.Add(curLine.Pt0);
                        lines.RemoveAt(i);
                        progress = true;
                        break;
                    }
                }
            }
        }

        private bool AlmostEqual(Point2D pt0, Point2D pt1, int delta = 1)
        {
            return (Math.Abs(pt0.X - pt1.X) + Math.Abs(pt0.Y - pt1.Y)) <= delta;
        }
    }
}