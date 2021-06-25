// unset

using Slicer.Models;

namespace Slicer.Slicer.PolygonOperations
{
    public interface ITriangulate
    {
        Polygons Triangulate(Polygons polygons);
    }
}