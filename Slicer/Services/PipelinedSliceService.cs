// unset

using Slicer.Middleware;
using Slicer.Models;
using Slicer.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Services
{
    public class PipelinedSliceService : ISliceService
    {
        private readonly IEnumerable<ISlicerStage> _stages;
        private readonly Project _project;

        public PipelinedSliceService(Project project, IEnumerable<ISlicerStage> stages)
        {
            _project = project;
            _stages = stages;
        }

        public async Task<string> Slice(SlicerServiceOptions options)
        {
            var slicerState = new SlicerState()
            {
                Project = _project,
                Options = options,
            };
            
            PipelineContainer<SlicerState> pipelineContainer = new PipelineContainer<SlicerState>(_stages);

            //var pipeline = pipelineContainer.Build(slicerState);
            var pipeline = pipelineContainer.BuildChain(slicerState);
            await pipeline();

            return slicerState.Gcode;
        }
    }
}