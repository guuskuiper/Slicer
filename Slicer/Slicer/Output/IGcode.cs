// unset

using Slicer.Models;
using System.Collections.Generic;

namespace Slicer.Slicer.Output
{
    public interface IGcode
    {
        string Create(IEnumerable<Layer> layers);
    }
}