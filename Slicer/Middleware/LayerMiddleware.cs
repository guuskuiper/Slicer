// unset

using Slicer.Slicer.Slice;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class LayerMiddleware : ISlicerMiddelware
    {
        private readonly ILayers _layers;

        public LayerMiddleware(ILayers layers)
        {
            _layers = layers;
        }

        public async Task Execute(SlicerState request, Func<Task> next)
        {
            request.Layers = _layers.CreateLayers(request.STL, true);
            
            await next();
        }
    }
}