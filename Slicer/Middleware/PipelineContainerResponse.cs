// unset

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class PipelineContainerResponse<TState, TResponse> : List<IMiddleware<TState, TResponse>>
    {
        public PipelineContainerResponse()
        {}
        
        public PipelineContainerResponse(IEnumerable<IMiddleware<TState, TResponse>> stages) : base(stages)
        {}

        public void AddStage(IMiddleware<TState, TResponse> stage)
        {
            this.Add(stage);
        }
        
        public NextResponseDelegate<TResponse> Build(TState state, TResponse initialResponse)
        {
            return this.AsEnumerable()
                .Reverse()
                .Aggregate<IMiddleware<TState, TResponse>, NextResponseDelegate<TResponse>>( 
                    () => Task.FromResult(initialResponse), 
                    (next, stage) => () => stage.Execute(state, next));
        }
        
        public NextResponseDelegate<TResponse> BuildChain(TState state)
        {
            NextResponseDelegate<TResponse> previous = () => Task.FromResult(default(TResponse));
            return () =>
            {
                foreach (IMiddleware<TState, TResponse> stage in this)
                {
                    previous = () => stage.Execute(state, previous);
                }

                return previous();
            };
            
        }
    }
}