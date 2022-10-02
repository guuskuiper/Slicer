// unset

using Slicer.Slicer.Slice;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class LayerStage : ISlicerStage, ISlicerStageResponse
    {
        private readonly ILayers _layers;

        public LayerStage(ILayers layers)
        {
            _layers = layers;
        }

        public Task Execute(SlicerState request, NextDelegate next)
        {
            request.Layers = _layers.CreateLayers(request.STL, true);
            
            return next();
        }

        public Task<SlicerResponse> Execute(SlicerState request, NextResponseDelegate<SlicerResponse> next)
        {
            request.Layers = _layers.CreateLayers(request.STL, true);

            return next();
        }
    }
}