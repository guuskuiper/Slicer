// unset

using Slicer.Slicer.Output;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class GcodeMiddleware : ISlicerMiddelware
    {
        private readonly IGcode _gcode;
        private readonly IFileIO _fileIO;

        public GcodeMiddleware(IGcode gcode, IFileIO fileIO)
        {
            _gcode = gcode;
            _fileIO = fileIO;
        }

        public async Task Execute(SlicerState request, Func<Task> next)
        {
            
            var gcode = _gcode.Create(request.Layers);
            await _fileIO.WriteTextAsync(request.Options.OutputFilePath, gcode);
        }
    }
}