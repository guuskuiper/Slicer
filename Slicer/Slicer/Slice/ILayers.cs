// unset

using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Slice
{
    public interface ILayers
    {
        List<Layer> CreateLayers(STL stl, bool parallel);
    }
}