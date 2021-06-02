// unset

using System.Threading.Tasks;

namespace Slicer.Slicer.Output
{
    public interface IFileIO
    {
        Task WriteTextAsync(string name, string content);
        Task<string> ReadTextAsync(string name);
    }
}