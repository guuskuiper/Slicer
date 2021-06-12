// unset

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Slicer.Utils
{
    public class ParallelScope : IParallelScope
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public ParallelScope(IServiceScopeFactory scopeFactory, ILogger logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void Parallelize<T, TS>(IEnumerable<T> items, Action<T, TS> action)
        {
            Parallel.ForEach(items, item =>
            {
                using var scope = _scopeFactory.CreateScope();
                _logger.Debug("Created scope {0} on thread {1} for {2}", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId, item);
                TS service = scope.ServiceProvider.GetRequiredService<TS>();
                action(item, service);
            });
        }

        public IEnumerable<TR> Parallelize<T, TS, TR>(IEnumerable<T> items, Func<T, TS, TR> func)
        {
            List<TR> results = new List<TR>();
            Parallel.ForEach(items, item =>
            {
                using var scope = _scopeFactory.CreateScope();
                _logger.Debug("Created scope {0} on thread {1} for {2}", scope.GetHashCode(), Thread.CurrentThread.ManagedThreadId, item);
                TS service = scope.ServiceProvider.GetRequiredService<TS>();
                results.Add(func(item, service));
            });
            return results;
        }
    }
}