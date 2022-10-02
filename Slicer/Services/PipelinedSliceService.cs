// unset

using Serilog;
using Slicer.Middleware;
using Slicer.Models;
using Slicer.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Services
{
    public class PipelinedSliceService : ISliceService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<ISlicerStageResponse> _responseStages;
        private readonly IEnumerable<ISlicerStage> _stages;
        private readonly Project _project;

        public PipelinedSliceService(ILogger logger, Project project, IEnumerable<ISlicerStage> stages, IEnumerable<ISlicerStageResponse> responseStages)
        {
            _logger = logger;
            _project = project;
            _stages = stages;
            _responseStages = responseStages;
        }

        public async Task<string> Slice(SlicerServiceOptions options)
        {
            _logger.Information($"Started slicing {options.InputFilePath}");
            
            var slicerState = new SlicerState()
            {
                Project = _project,
                Options = options,
            };

            var initialResponse = new SlicerResponse();

            PipelineContainerResponse<SlicerState, SlicerResponse> pipelineContainerResponse = new PipelineContainerResponse<SlicerState, SlicerResponse>(_responseStages);
            PipelineContainer<SlicerState> pipelineContainer = new PipelineContainer<SlicerState>(_stages);

            //var pipeline = pipelineContainer.Build(slicerState);
            //var pipeline = pipelineContainer.BuildChain(slicerState);
            //await pipeline();
            var pipeline = pipelineContainerResponse.Build(slicerState, initialResponse);
            var finalResponse = await pipeline();
            
            _logger.Information($"Save Gcode to {options.OutputFilePath}");

            return slicerState.Gcode;
        }
    }
}