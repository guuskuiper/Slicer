// unset

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace CustomDI
{
    public class ServiceScope : IServiceScope, IServiceProvider, ISupportRequiredService
    {
        private readonly ServiceProvider _baseServiceProvider;
        internal readonly ConcurrentDictionary<Type, object> BuildScopedServices = new ConcurrentDictionary<Type, object>();

        public ServiceScope(ServiceProvider baseServiceProvider)
        {
            _baseServiceProvider = baseServiceProvider;
        }

        public void Dispose()
        {
            foreach (var buildScopedService in BuildScopedServices)
            {
                if(buildScopedService.Value is IDisposable disposable) disposable.Dispose();
            }
        }

        public IServiceProvider ServiceProvider => this;

        private object GetServiceCore(Type serviceType)
        {
            return _baseServiceProvider.GetServiceScoped(serviceType, this);
        }

        public object? GetService(Type serviceType)
        {
            return GetServiceCore(serviceType);
        }

        public object GetRequiredService(Type serviceType)
        {
            return GetServiceCore(serviceType);
        }

        public T GetService<T>(Type serviceType)
        {
            return (T)GetServiceCore(serviceType);
        }
    }
}