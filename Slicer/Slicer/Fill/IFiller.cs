// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill
{
    public interface IFiller
    {
        void Fill(Layer layer);
        void FillOutside(Layer layer);
    }
}