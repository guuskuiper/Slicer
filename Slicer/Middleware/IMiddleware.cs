// unset

using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public interface IMiddleware<TRequest, TResponse>
    {
        public Task<TResponse> Execute(TRequest request, NextResponseDelegate<TResponse> next);
    }
    
    public delegate Task<TResponse> NextResponseDelegate<TResponse>();
    public delegate Task NextDelegate();
    
    public interface IMiddleware<in TRequest>
    {
        public Task Execute(TRequest request, NextDelegate next);
    }
}