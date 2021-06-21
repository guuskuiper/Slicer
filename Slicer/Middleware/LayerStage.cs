// unset

using Slicer.Slicer.Slice;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class LayerStage : ISlicerStage
    {
        private readonly ILayers _layers;

        public LayerStage(ILayers layers)
        {
            _layers = layers;
        }

        public async Task Execute(SlicerState request, NextDelegate next)
        {
            request.Layers = _layers.CreateLayers(request.STL, true);
            
            await next();
        }
    }
}