// unset

using Slicer.Models;

namespace Slicer.Slicer.PolygonOperations.Triangulation
{
    public interface ITriangulate
    {
        Polygons Triangulate(Polygons polygons);
    }
}