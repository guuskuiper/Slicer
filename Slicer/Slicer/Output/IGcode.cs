// unset

using Slicer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Slicer.Slicer.Output
{
    public interface IGcode
    {
        string Create(IEnumerable<Layer> layers);
    }
}