// unset

using Slicer.Models;
using Slicer.Slicer.Input;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class STLStage : ISlicerStage, ISlicerStageResponse
    {
        private readonly STLConverter _stlConverter;

        public STLStage(STLConverter stlConverter)
        {
            _stlConverter = stlConverter;
        }

        public Task Execute(SlicerState request, NextDelegate next)
        {
            // pre
            request.STL = _stlConverter.Read(request.Options.InputFilePath);
            
            // next
            return next();
            
            // post            
        }

        public async Task<SlicerResponse> Execute(SlicerState request, NextResponseDelegate<SlicerResponse> next)
        {
            request.STL = _stlConverter.Read(request.Options.InputFilePath);

            var response = await next();

            return response;
        }
    }
}