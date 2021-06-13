using Slicer.Options;
using System.Threading.Tasks;

namespace Slicer.Services
{
    public interface ISliceService
    {
        Task<string> Slice(SlicerServiceOptions options);
    }
}