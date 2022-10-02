// unset

using Slicer.Models;
using Slicer.Slicer.Sort;
using Slicer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class SortingStage : ISlicerStage, ISlicerStageResponse
    {
        private readonly IParallelScope _parallelScope;
        private readonly ISort _sort;

        public SortingStage(IParallelScope parallelScope, ISort sort)
        {
            _parallelScope = parallelScope;
            _sort = sort;
        }

        public Task Execute(SlicerState request, NextDelegate next)
        {
            Sort(request.Layers, request.Options.Parallel);
            return next();
        }
        
        public Task<SlicerResponse> Execute(SlicerState request, NextResponseDelegate<SlicerResponse> next)
        {
            Sort(request.Layers, request.Options.Parallel);
            return next();
        }
        
        private void Sort(List<Layer> layers, bool parallel)
        {
            if (parallel)
            {
                _parallelScope.Parallelize<Layer, ISort>(layers, (layer, sort) => sort.SortPolygonsInplace(layer, new Point2D(0, 0)));
            }
            else
            {
                Point2D curPt = new (0, 0);
                foreach (Layer layer in layers)
                {
                    var lastPt = _sort.SortPolygonsInplace(layer, curPt);
                    
                    curPt = lastPt;
                }
            }
        }
    }
}