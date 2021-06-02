// unset

using Slicer.Models;

namespace Slicer.Slicer.Slice
{
    public interface ISliceLayer
    {
        void CreateLayerContour(Layer layer, STL stl);
    }
}