// unset

using Slicer.Models;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Slicer.Slicer.Input
{
    public class STLConverter
    {
        public STL Read(string path)
        {
            var stl = new STL();
            var file = File.Open(path, FileMode.Open);
            using BinaryReader b = new BinaryReader(file, Encoding.ASCII);
            stl.Header = new string(b.ReadChars(80));
            stl.NumerOfTriangles = b.ReadUInt32();
            stl.Triangles = new List<Triangle>();

            for (int i = 0; i < stl.NumerOfTriangles; i++)
            {
                stl.Triangles.Add(ReadTriangle(b));
            }

            return stl;
        }

        private Triangle ReadTriangle(BinaryReader b)
        {
            var triangle = new Triangle();
            triangle.Normal = ReadVector3(b);
            triangle.Vertex1 = ReadVector3(b);
            triangle.Vertex2 = ReadVector3(b);
            triangle.Vertex3 = ReadVector3(b);
            triangle.AttributeByteCount = b.ReadUInt16();
            return triangle;
        }

        private Vector3 ReadVector3(BinaryReader b)
        {
            var v3 = new Vector3();
            v3.X = b.ReadSingle();
            v3.Y = b.ReadSingle();
            v3.Z = b.ReadSingle();
            return v3;
        }
    }
}