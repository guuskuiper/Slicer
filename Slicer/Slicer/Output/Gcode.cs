﻿// unset

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
        private readonly IGcodeCommands _gcodeCommands;
        private readonly Project _project;

        private readonly StringBuilder _sb = new();
        private readonly GcodeState _state;

        public Gcode(Project project, IGcodeCommands gcodeCommands, GcodeState state)
        {
            _project = project;
            _gcodeCommands = gcodeCommands;
            _state = state;
        }

        public string Create(IOrderedEnumerable<SortedLayer> layers)
        {
            foreach (SortedLayer layer in layers)
            {
                CreateLayer(layer);
            }

            return _sb.ToString();
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
            _sb.AppendLine(_gcodeCommands.TravelZ(z, speed));
        }

        private void TravelXY(IntPoint pt, double speed)
        {
            _sb.AppendLine(_gcodeCommands.TravelXY(pt, speed));
            _state.Pt = pt;
        }

        private void Extruder(IntPoint pt, double volume, double speed)
        {
            _state.E += volume;
            _sb.AppendLine(_gcodeCommands.Extrude(pt, _state.E, speed));
            _state.Pt = pt;
        }

        private void PrintPath(Polygon path, double thickness)
        {
            if(path.Poly.Count == 0) return;

            IntPoint nextPt = path.Poly[0];

            if (_state.Pt != nextPt)
            {
                TravelXY(nextPt, _project.setting.TravelSpeed);
            }

            foreach (IntPoint pt in path.Poly.Skip(1))
            {
                var e = CalcVolume(_state.Pt, pt, thickness, _project.setting.LineWidth / 1000.0);

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