using Serilog;
using Slicer.Models;
using Slicer.Slicer.Fill;
using Slicer.Slicer.Input;
using Slicer.Slicer.Output;
using Slicer.Slicer.Slice;
using Slicer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Services
{
    public class SliceService : ISliceService
    {
        private readonly ILogger _logger;
        private readonly IFileIO _fileIO;
        private readonly ILayers _layers;
        private readonly IFiller _filler;
        private readonly Project _project;
        private readonly IGcode _gcode;
        private readonly STLConverter _stlConverter;
        private readonly IParallelScope _parallelScope;

        public SliceService(ILogger logger, IFileIO fileIO, ILayers layers, IFiller filler, Project project, IGcode gcode, STLConverter stlConverter, IParallelScope parallelScope)
        {
            _logger = logger;
            _fileIO = fileIO;
            _layers = layers;
            _filler = filler;
            _project = project;
            _gcode = gcode;
            _stlConverter = stlConverter;
            _parallelScope = parallelScope;
        }

        public async Task Slice(string inputFilePath, string outputFilePath, bool parallel = true)
        {
            _logger.Information($"Started slicing {inputFilePath}");

            /* read input */
            var stl = _stlConverter.Read(inputFilePath);

            /* create layers */
            var layers = _layers.CreateLayers(stl, parallel);

            /* fill layers */
            Fill(layers, parallel);

            /* add buildplate adhesion */
            _filler.FillOutside(layers[0]);

            /* sort? */

            /* convert to output and save */
            await Output(outputFilePath, layers);

            _logger.Information($"Save Gcode to {outputFilePath}");
        }

        private async Task Output(string outputFilePath, List<Layer> layers)
        {
            var gcode = _gcode.Create(layers);
            await _fileIO.WriteTextAsync(outputFilePath, gcode);
        }

        private void Fill(List<Layer> layers, bool parallel)
        {
            if (false && parallel)
            {
                _parallelScope.Parallelize<Layer, IFiller>(layers, (layer, filler) => filler.Fill(layer));
            }
            else
            {
                foreach (Layer layer in layers)
                {
                    _filler.Fill(layer);
                }
            }
        }
    }
}