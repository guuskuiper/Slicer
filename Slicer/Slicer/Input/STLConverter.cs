// unset

using Slicer.Models;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Slicer.Slicer.Input
{
    public class STLConverter
    {
        public STL Read(string path)
        {
            var file = File.Open(path, FileMode.Open);
            return ReadBinarySTL(file); 
        }

        public void Write(string path, STL stl)
        {
            var file = File.Open(path, FileMode.CreateNew);
            WriteBinarySTL(file, stl);
        }

        public STL ReadBinarySTL(Stream file)
        {
            using BinaryReader b = new BinaryReader(file, Encoding.ASCII);
            var stl = new STL();
            stl.Header = new string(b.ReadChars(80));
            stl.NumerOfTriangles = b.ReadUInt32();
            stl.Triangles = new List<STLTriangle>();

            for (int i = 0; i < stl.NumerOfTriangles; i++)
            {
                stl.Triangles.Add(ReadTriangle(b));
            }

            return stl;
        }

        public void WriteBinarySTL(Stream file, STL stl)
        {
            using BinaryWriter w = new BinaryWriter(file, Encoding.ASCII, true);
            byte[] bytes =  Encoding.ASCII.GetBytes(stl.Header.PadRight(80, '\0'));
            w.Write(bytes);
            w.Write(stl.NumerOfTriangles);
            foreach (STLTriangle stlTriangle in stl.Triangles)
            {
                WriteTriangle(w, stlTriangle);
            }

            w.Flush();
        }

        private STLTriangle ReadTriangle(BinaryReader b)
        {
            var triangle = new STLTriangle();
            triangle.Normal = ReadVector3(b);
            triangle.Vertex1 = ReadVector3(b);
            triangle.Vertex2 = ReadVector3(b);
            triangle.Vertex3 = ReadVector3(b);
            triangle.AttributeByteCount = b.ReadUInt16();
            return triangle;
        }

        private void WriteTriangle(BinaryWriter w, STLTriangle t)
        {
            WriteVector3(w, t.Normal);
            WriteVector3(w, t.Vertex1);
            WriteVector3(w, t.Vertex2);
            WriteVector3(w, t.Vertex3);
            w.Write(t.AttributeByteCount);
        }

        private Vector3 ReadVector3(BinaryReader b)
        {
            var v3 = new Vector3();
            v3.X = b.ReadSingle();
            v3.Y = b.ReadSingle();
            v3.Z = b.ReadSingle();
            return v3;
        }

        private void WriteVector3(BinaryWriter w, Vector3 v)
        {
            w.Write(v.X);
            w.Write(v.Y);
            w.Write(v.Z);
        }
    }
}