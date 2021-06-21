// unset

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class PipelineContainer<TState> : List<IMiddleware<TState>>
    {
        public PipelineContainer()
        {}
        
        public PipelineContainer(IEnumerable<IMiddleware<TState>> stages) : base(stages)
        {}

        public void AddStage(IMiddleware<TState> stage)
        {
            this.Add(stage);
        }
        
        public NextDelegate Build(TState state)
        {
            return this.AsEnumerable()
                .Reverse()
                .Aggregate<IMiddleware<TState>, NextDelegate>( 
                    () => Task.CompletedTask, 
                    (next, stage) => () => stage.Execute(state, next));
        }
        
        public NextDelegate BuildChain(TState state)
        {
            NextDelegate empty = () => Task.CompletedTask;
            return async () =>
            {
                foreach (IMiddleware<TState> stage in this)
                {
                    await stage.Execute(state, empty);
                }
            };
        }
    }
}