using Serilog;
using Slicer.Models;
using Slicer.Options;
using Slicer.Slicer.Fill;
using Slicer.Slicer.Input;
using Slicer.Slicer.Output;
using Slicer.Slicer.Slice;
using Slicer.Slicer.Sort;
using Slicer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slicer.Services
{
    public class SliceService : ISliceService
    {
        private readonly ILogger _logger;
        private readonly IFileIO _fileIO;
        private readonly ILayers _layers;
        private readonly IFiller _filler;
        private readonly ISort   _sort;
        private readonly Project _project;
        private readonly IGcode  _gcode;
        private readonly STLConverter _stlConverter;
        private readonly IParallelScope _parallelScope;

        public SliceService(ILogger logger, IFileIO fileIO, ILayers layers, IFiller filler, Project project, IGcode gcode, STLConverter stlConverter, IParallelScope parallelScope, ISort sort)
        {
            _logger = logger;
            _fileIO = fileIO;
            _layers = layers;
            _filler = filler;
            _project = project;
            _gcode = gcode;
            _stlConverter = stlConverter;
            _parallelScope = parallelScope;
            _sort = sort;
        }

        public async Task<string> Slice(SlicerServiceOptions options)
        {
            _logger.Information($"Started slicing {options.InputFilePath}");

            /* read input */
            var stl = _stlConverter.Read(options.InputFilePath);

            /* create layers */
            var layers = _layers.CreateLayers(stl, options.Parallel);

            /* fill layers */
            Fill(layers, options.Parallel);

            /* add buildplate adhesion */
            _filler.FillOutside(layers[0]);

            /* sort */
            var sortedLayers = Sort(layers, options.Parallel);

            /* convert to output and save */
            await Output(options.OutputFilePath, sortedLayers);

            _logger.Information($"Save Gcode to {options.OutputFilePath}");

            return "";
        }

        private async Task Output(string outputFilePath, IOrderedEnumerable<Layer> layers)
        {
            var gcode = _gcode.Create(layers);
            await _fileIO.WriteTextAsync(outputFilePath, gcode);
        }

        private void Fill(List<Layer> layers, bool parallel)
        {
            if (parallel)
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

        private IOrderedEnumerable<Layer> Sort(List<Layer> layers, bool parallel)
        {
            List<Layer> sortedLayers = new List<Layer>(layers.Capacity);
            if (parallel)
            {
                sortedLayers.AddRange(_parallelScope.Parallelize<Layer, ISort, Layer>(layers, (layer, sort) => sort.SortPolygons(layer, new Point2D(0, 0)).Item1));
            }
            else
            {
                Point2D curPt = new (0, 0);
                foreach (Layer layer in layers)
                {
                    var (sortedLayer, lastPt) = _sort.SortPolygons(layer, curPt);
                    
                    curPt = lastPt;
                    sortedLayers.Add(sortedLayer);
                }
            }

            IOrderedEnumerable<Layer> orderedEnumerable = sortedLayers.OrderBy(x => x.Height);

            return orderedEnumerable;
        }
    }
}