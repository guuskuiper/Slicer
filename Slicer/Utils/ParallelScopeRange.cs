// unset

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Slicer.Utils
{
    public class ParallelScopeRange : IParallelScope
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public ParallelScopeRange(IServiceScopeFactory scopeFactory, ILogger logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void Parallelize<T, TS>(IEnumerable<T> items, Action<T, TS> action)
        {
            //Partitioner.Create(items, op)
            //var partitionerRange = Partitioner.Create(0, items.Count());
            Partitioner<T> partitioner = Partitioner.Create(items);
            //ThreadPool.GetMaxThreads(out int workerThreads, out int ioThreads);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
                

            Parallel.ForEach(partitioner, (item, state) =>
            {
                using var scope = _scopeFactory.CreateScope();
                _logger.Debug("Created scope {0} on thread {1} for {2}", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId, item);
                TS service = scope.ServiceProvider.GetRequiredService<TS>();
                action(item, service);
            });

        }
    }
}