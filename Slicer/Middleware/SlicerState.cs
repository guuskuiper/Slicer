// unset

using Slicer.Models;
using Slicer.Options;
using System.Collections.Generic;

namespace Slicer.Middleware
{
    public class SlicerState
    {
        public Project Project;
        public SlicerServiceOptions Options;
        public STL STL;
        public List<Layer> Layers;
        public string Gcode;
    }
}