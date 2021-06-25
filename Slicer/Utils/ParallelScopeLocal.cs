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
    public class ParallelScopeLocal: IParallelScope
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public ParallelScopeLocal(IServiceScopeFactory scopeFactory, ILogger logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void Parallelize<T, TS>(IEnumerable<T> items, Action<T, TS> action)
        {
            /* Create 1 scope / service per thread */ 
            Parallel.ForEach(items,
                () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    _logger.Debug("Created scope {0} on thread {1} for", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId);
                    TS service = scope.ServiceProvider.GetRequiredService<TS>();
                    return service;
                },
                (item, loopState, service) =>
                {
                    action(item, service);
                    return service;
                },
                s => { } );
            
            /* Are the scopes disposed?*/ 
        }

        public IEnumerable<TR> Parallelize<T, TS, TR>(IEnumerable<T> items, Func<T, TS, TR> func)
        {
            List<TR> results = new List<TR>();

            /* Create 1 scope / service per thread */
            Parallel.ForEach(items,
                () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    _logger.Debug("Created scope {0} on thread {1} for", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId);
                    TS service = scope.ServiceProvider.GetRequiredService<TS>();
                    return service;
                },
                (item, loopState, service) =>
                {
                    results.Add(func(item, service));
                    return service;
                },
                s => { });
            /* Are the scopes disposed?*/

            return results;
        }
        
        
        public IEnumerable<TR> ParallelizeAsync<T, TS, TR>(IEnumerable<T> items, Func<T, TS, TR> func)
        {
            return Partitioner.Create(items)
                .GetPartitions(Environment.ProcessorCount)
                .AsParallel()
                .SelectMany( x => ParallelInner<TR, T, TS>(x, func));
        }
        
        public async Task<IEnumerable<TR>> ParallelizeAsync<T, TS, TR>(IEnumerable<T> items, Func<T, TS, Task<TR>> func)
        {
            return (await Task.WhenAll(Partitioner.Create(items)
                .GetPartitions(Environment.ProcessorCount)
                .AsParallel()
                .Select(x => ParallelInnerAsync(x, func)))).SelectMany(xx => xx);
        }

        private IEnumerable<TR> ParallelInner<TR, T, TS>(IEnumerator<T> partition, Func<T, TS, TR> convert)
        {
            using var scope = _scopeFactory.CreateScope();
            _logger.Debug("Created scope {0} on thread {1}", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId);
            TS service = scope.ServiceProvider.GetRequiredService<TS>();
            using (partition)
            {
                while (partition.MoveNext())
                {
                    yield return convert(partition.Current, service);
                }
            }
        }
        
        private async Task<IEnumerable<TR>> ParallelInnerAsync<TR, T, TS>(IEnumerator<T> partition, Func<T, TS, Task<TR>> convert)
        {
            List<TR> result = new();
            using var scope = _scopeFactory.CreateScope();
            _logger.Debug("Created scope {0} on thread {1}", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId);
            TS service = scope.ServiceProvider.GetRequiredService<TS>();
            using (partition)
            {
                while (partition.MoveNext())
                {
                    result.Add(await convert(partition.Current, service));
                }
            }

            return result;
        }
    }
}