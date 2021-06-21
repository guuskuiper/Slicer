using System;
using System.IO;
using System.Threading.Tasks;

namespace Slicer.Slicer.Output
{
    public class FileIO : IFileIO
    {
        public Task WriteTextAsync(string path, string content)
        {
            return File.WriteAllTextAsync(path, content);
        }

        public Task<string> ReadTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }

        public Task WriteBytesAsync(string name, byte[] data)
        {
            return File.WriteAllBytesAsync(name, data);
        }

        public Task<byte[]> ReadBytesAsync(string name)
        {
            return File.ReadAllBytesAsync(name);
        }

        public async Task WriteStreamAsync(string path, Func<Stream, Task> writer)
        {
            await using var sourceStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await writer(sourceStream);
        }

        public async Task<T> ReadStreamAsync<T>(string path, Func<Stream, Task<T>> reader)
        {
            await using var sourceStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize: 4096, useAsync: true);
            return await reader(sourceStream);
        }
    }
}