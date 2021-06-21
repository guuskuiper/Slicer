// unset

using Slicer.Models;
using Slicer.Slicer.Input;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class STLStage : ISlicerStage
    {
        private readonly STLConverter _stlConverter;

        public STLStage(STLConverter stlConverter)
        {
            _stlConverter = stlConverter;
        }

        public async Task Execute(SlicerState request, NextDelegate next)
        {
            // pre
            request.STL = _stlConverter.Read(request.Options.InputFilePath);
            
            // next
            await next();
            
            // post            
        }
    }
}