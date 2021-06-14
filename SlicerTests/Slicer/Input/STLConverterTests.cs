using FluentAssertions;
using Slicer.Models;
using Xunit;
using Slicer.Slicer.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Slicer.Input.Tests
{
    public class STLConverterTests
    {
        [Fact]
        public void ReadWriteTest()
        {
            // Arrange
            var stl = new STL()
            {
                Header = "Test STL".PadRight(80),
                NumerOfTriangles = 1,
                Triangles = new List<Triangle>()
                {
                    new Triangle()
                    {
                        Normal = Vector3.UnitZ,
                        Vertex1 = Vector3.UnitY,
                        Vertex2 = Vector3.UnitX,
                        Vertex3 = Vector3.UnitY + Vector3.UnitX,
                        AttributeByteCount = 0
                    }
                }
            };

            var stlConverter = new STLConverter();
            var memoryWrite = new MemoryStream();

            // Act
            stlConverter.WriteBinarySTL(memoryWrite, stl);
            byte[] buff = memoryWrite.ToArray();
            
            var memoryRead = new MemoryStream(buff, false);
            var readSTL = stlConverter.ReadBinarySTL(memoryRead);

            // Assert 
            readSTL.Should().BeEquivalentTo(stl);
        }
    }
}