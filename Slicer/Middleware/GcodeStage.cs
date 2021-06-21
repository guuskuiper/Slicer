// unset

using Slicer.Slicer.Output;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class GcodeStage : ISlicerStage
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
        }
    }
}