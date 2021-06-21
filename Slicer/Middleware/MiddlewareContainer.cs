// unset

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public class MiddlewareContainer : List<ISlicerMiddelware>
    {
        public Func<Task> Build(SlicerState state)
        {
            return Generate(state);
        }

        private Func<Task> GenerateAggregate(IEnumerable<ISlicerMiddelware> enumerable, SlicerState state)
        {
            enumerable.Reverse().Aggregate<ISlicerMiddelware, Task>(Task.CompletedTask, (next, middelware) => middelware.Execute(state, next));
        }
        
        private Func<Task> Generate(SlicerState state)
        {
            List<ISlicerMiddelware> reverseMiddleware = new(this);
            reverseMiddleware.Reverse();

            Func<Task> nextTask = () => Task.CompletedTask;
            
            foreach (ISlicerMiddelware middleware in reverseMiddleware)
            {
                Func<Task> task = nextTask;
                Task CurrentTask() => middleware.Execute(state, task);
                nextTask = CurrentTask;
            }

            return nextTask;
        }
        
    }
}