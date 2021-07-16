// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill
{
    public record FillResult(Polygons Paths, Polygons RemainingArea);
}