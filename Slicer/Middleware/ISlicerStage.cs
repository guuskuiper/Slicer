// unset

namespace Slicer.Middleware
{
    public interface ISlicerStage : IMiddleware<SlicerState>
    {
    }
    
    public interface ISlicerStageResponse : IMiddleware<SlicerState, SlicerResponse>
    {
    }
}