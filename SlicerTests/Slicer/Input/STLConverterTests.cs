using FluentAssertions;
using Slicer.Models;
using Xunit;
using Slicer.Slicer.Output;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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
                Triangles = new List<STLTriangle>()
                {
                    new STLTriangle()
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

        [Fact]
        public async Task ReadWriteFileTest()
        {
            // Arrange
            var stl = new STL()
            {
                Header = "Test STL".PadRight(80),
                NumerOfTriangles = 1,
                Triangles = new List<STLTriangle>()
                {
                    new STLTriangle()
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
            STL readSTL;
            
            // Act

            await using (var stream = new MemoryStream())
            {
                stlConverter.WriteBinarySTL(stream, stl);
                stream.Position = 0;
                readSTL = stlConverter.ReadBinarySTL(stream);
            }

            // Assert 
            readSTL.Should().BeEquivalentTo(stl);


            var s = File.Create("test.stl");
        }

        [Fact]
        public async Task ReadWriteFileStreamTest()
        {
            // Arrange
            var stl = new STL()
            {
                Header = "Test STL".PadRight(80),
                NumerOfTriangles = 1,
                Triangles = new List<STLTriangle>()
                {
                    new STLTriangle()
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

            // Act
            await using (var s = File.Create("test.stl"))
            {
                stlConverter.WriteBinarySTL(s, stl);
            }

            STL readSTL;

            await using (var s = File.OpenRead("test.stl"))
            {
                readSTL = stlConverter.ReadBinarySTL(s);
            }

            // Assert 
            readSTL.Should().BeEquivalentTo(stl);
        }

        [Fact]
        public async Task ReadWriteFileFuncStreamTest()
        {
            // Arrange
            var stl = new STL()
            {
                Header = "Test STL".PadRight(80),
                NumerOfTriangles = 1,
                Triangles = new List<STLTriangle>()
                {
                    new STLTriangle()
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
            IFileIO fileIO = new FileIO();

            // Act
            await fileIO.WriteStreamAsync("test.stl", stream =>
            {
                stlConverter.WriteBinarySTL(stream, stl);
                return Task.CompletedTask;
            });
            
            STL readSTL = await fileIO.ReadStreamAsync("test.stl", stream => Task.FromResult(stlConverter.ReadBinarySTL(stream)));

            // Assert 
            readSTL.Should().BeEquivalentTo(stl);
        }
    }
}