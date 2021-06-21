// unset

using Slicer.Models;
using Slicer.Slicer.Fill;
using Slicer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class FillMiddleware : ISlicerMiddelware
    {
        private readonly IParallelScope _parallelScope;
        private readonly IFiller _filler;

        public FillMiddleware(IParallelScope parallelScope, IFiller filler)
        {
            _parallelScope = parallelScope;
            _filler = filler;
        }

        public async Task Execute(SlicerState request, Func<Task> next)
        {
            Fill(request.Layers, request.Options.Parallel);
            
            await next();
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
    }
}