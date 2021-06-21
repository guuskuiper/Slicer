// unset

using ClipperLib;
using Slicer.Models;
using Slicer.Slicer.Sort;
using Slicer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class SortingStage : ISlicerStage
    {
        private readonly IParallelScope _parallelScope;
        private readonly ISort _sort;

        public SortingStage(IParallelScope parallelScope, ISort sort)
        {
            _parallelScope = parallelScope;
            _sort = sort;
        }

        public async Task Execute(SlicerState request, NextDelegate next)
        {
            Sort(request.Layers, request.Options.Parallel);
            await next();
        }
        
        private void Sort(List<Layer> layers, bool parallel)
        {
            if (parallel)
            {
                _parallelScope.Parallelize<Layer, ISort>(layers, (layer, sort) => sort.SortPolygonsInplace(layer, new IntPoint(0, 0)));
            }
            else
            {
                IntPoint curPt = new IntPoint(0, 0);
                foreach (Layer layer in layers)
                {
                    var lastPt = _sort.SortPolygonsInplace(layer, curPt);
                    
                    curPt = lastPt;
                }
            }
        }
    }
}