// unset

using System;
using System.IO;
using System.Threading.Tasks;

namespace Slicer.Slicer.Output
{
    public interface IFileIO
    {
        Task WriteTextAsync(string name, string content);
        Task<string> ReadTextAsync(string name);
        Task WriteBytesAsync(string name, byte[] data);
        Task<byte[]> ReadBytesAsync(string name);
        Task WriteStreamAsync(string path, Func<Stream, Task> writer);
        Task<T> ReadStreamAsync<T>(string path, Func<Stream, Task<T>> reader);
    }
}