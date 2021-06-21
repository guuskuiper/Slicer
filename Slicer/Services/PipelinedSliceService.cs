// unset

using Slicer.Middleware;
using Slicer.Models;
using Slicer.Options;
using Slicer.Slicer.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Services
{
    public class PipelinedSliceService : ISliceService
    {
        private readonly IEnumerable<ISlicerMiddelware> _middelwares;
        private readonly Project _project;

        public PipelinedSliceService(Project project, IEnumerable<ISlicerMiddelware> middelwares)
        {
            _project = project;
            _middelwares = middelwares;
        }

        public async Task<string> Slice(SlicerServiceOptions options)
        {
            var slicerState = new SlicerState()
            {
                Project =  _project,
                Options = options,
            };
            MiddlewareContainer middlewareContainer = new MiddlewareContainer();
            middlewareContainer.AddRange(_middelwares);

            var pipeline = middlewareContainer.Build(slicerState);
            await pipeline();

            return slicerState.Gcode;
        }
    }
}