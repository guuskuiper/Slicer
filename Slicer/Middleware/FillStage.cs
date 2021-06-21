﻿// unset

using Slicer.Models;
using Slicer.Slicer.Fill;
using Slicer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class FillStage : ISlicerStage
    {
        private readonly IParallelScope _parallelScope;
        private readonly IFiller _filler;

        public FillStage(IParallelScope parallelScope, IFiller filler)
        {
            _parallelScope = parallelScope;
            _filler = filler;
        }

        public async Task Execute(SlicerState request, NextDelegate next)
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