// unset

using ClipperLib;
using Slicer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slicer.Slicer.Output
{
    public class Gcode : IGcode
    {
        private StringBuilder sb = new StringBuilder();
        private IntPoint currentPt = new IntPoint();
        private Project _project;
        private double currentE = 0;

        public Gcode(Project project)
        {
            _project = project;
        }

        public string Create(IOrderedEnumerable<SortedLayer> layers)
        {
            foreach (SortedLayer layer in layers)
            {
                CreateLayer(layer);
            }

            return sb.ToString();
        }

        private void CreateLayer(SortedLayer layer)
        {
            TravelZ(layer.Height, _project.setting.TravelSpeed);
            foreach (var path in layer.Paths.Polys)
            {
                PrintPath(path, layer.Thickness);
            }
        }

        private void TravelZ(double z, double speed)
        {
            sb.AppendLine($"G0 Z{z:F3} F{speed:F3}");
        }

        private void TravelXY(IntPoint pt, double speed)
        {
            sb.AppendLine($"G0 X{(pt.X / 1000.0):F3} Y{(pt.Y / 1000.0):F3} F{speed:F3}");
            currentPt = pt;
        }

        private void Extruder(IntPoint pt, double volume, double speed)
        {
            currentE += volume;
            sb.AppendLine($"G1 X{(pt.X / 1000.0):F3} Y{(pt.Y / 1000.0):F3} F{speed:F3} E{currentE:F3}");
            currentPt = pt;
        }

        private void PrintPath(Polygon path, double thickness)
        {
            if(path.Poly.Count == 0) return;

            IntPoint nextPt = path.Poly[0];

            if (currentPt != nextPt)
            {
                TravelXY(nextPt, _project.setting.TravelSpeed);
            }

            foreach (IntPoint pt in path.Poly.Skip(1))
            {
                var e = CalcVolume(currentPt, pt, thickness, _project.setting.LineWidth / 1000.0);

                Extruder(pt, e, _project.setting.PrintSpeed);
            }
        }

        // in mm^3
        private double CalcVolume(IntPoint from, IntPoint to, double thickness, double width)
        {
            double dx = (to.X - from.X);
            double dy = (to.Y - from.Y);
            double length = Math.Sqrt(dx * dx + dy * dy) / 1000;

            return length * thickness * width;
        }
    }
}