// unset

using MediatR;
using Serilog;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SlicerMediatR.PipelineBehaviours
{
    public class TimingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public TimingBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            
            TResponse response = await next();
            
            sw.Stop();
            _logger.Information($"Slicing took {sw.ElapsedMilliseconds}ms");
            return response;
        }
    }
}