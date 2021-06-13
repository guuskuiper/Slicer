using System.Threading.Tasks;
using CommandLine;
using Slicer.Options;
using Slicer.Services;

namespace ConsoleSlicerDI
{
    public class SlicerApplication
    {
        private readonly ISliceService _sliceService;

        public SlicerApplication(ISliceService sliceService)
        {
            _sliceService = sliceService;
        }

        public async Task RunAsync(string[] args)
        {
            await Parser.Default
                .ParseArguments<SlicerApplicationOption>(args)
                .WithParsedAsync(
                    async option => 
                        await _sliceService.Slice(new SlicerServiceOptions()
                        {
                            InputFilePath = option.Infile,
                            OutputFilePath = option.Outfile,
                            Parallel = option.Parallel
                        }));
        }
    }
}