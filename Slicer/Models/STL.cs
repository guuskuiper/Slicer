// unset

using System;
using System.Collections.Generic;
using System.Numerics;

namespace Slicer.Models
{
    /*
     * UINT8[80]    – Header                 -     80 bytes                           
       UINT32       – Number of triangles    -      4 bytes
       
       foreach triangle                      - 50 bytes:
           REAL32[3] – Normal vector             - 12 bytes
           REAL32[3] – Vertex 1                  - 12 bytes
           REAL32[3] – Vertex 2                  - 12 bytes
           REAL32[3] – Vertex 3                  - 12 bytes
           UINT16    – Attribute byte count      -  2 bytes
       end
     */
    [Serializable]
    public class STL
    {
        public string Header { get; set; }
        public UInt32 NumerOfTriangles { get; set; }
        public List<STLTriangle> Triangles { get; set; }
    }

    [Serializable]
    public class STLTriangle
    {
        public Vector3 Normal { get; set; }
        public Vector3 Vertex1 { get; set; }
        public Vector3 Vertex2 { get; set; }
        public Vector3 Vertex3 { get; set; }
        public UInt16 AttributeByteCount { get; set; }
    }
}