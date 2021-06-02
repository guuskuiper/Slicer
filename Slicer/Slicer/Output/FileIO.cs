using System.IO;
using System.Threading.Tasks;

namespace Slicer.Slicer.Output
{
    public class FileIO : IFileIO
    {
        public async Task WriteTextAsync(string path, string content)
        {
            await File.WriteAllTextAsync(path, content);
        }

        public async Task<string> ReadTextAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }
    }
}