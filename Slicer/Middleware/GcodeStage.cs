// unset

using Slicer.Slicer.Output;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class GcodeStage : ISlicerStage, ISlicerStageResponse
    {
        private readonly IGcode _gcode;
        private readonly IFileIO _fileIO;

        public GcodeStage(IGcode gcode, IFileIO fileIO)
        {
            _gcode = gcode;
            _fileIO = fileIO;
        }

        public async Task Execute(SlicerState request, NextDelegate next)
        {
            request.Gcode = _gcode.Create(request.Layers);
            await _fileIO.WriteTextAsync(request.Options.OutputFilePath, request.Gcode);

            await next();
        }

        public async Task<SlicerResponse> Execute(SlicerState request, NextResponseDelegate<SlicerResponse> next)
        {
            var gcode  = _gcode.Create(request.Layers);
            await _fileIO.WriteTextAsync(request.Options.OutputFilePath, gcode);
            
            var response = await next();

            response.Gcode = gcode;
            return response;
        }
    }
}