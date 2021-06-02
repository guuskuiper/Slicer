using System.Threading.Tasks;

namespace Slicer.Services
{
    public interface ISliceService
    {
        Task Slice(string inputFilePath, string outputFilePath, bool parallel = true);
    }
}