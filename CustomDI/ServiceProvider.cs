// unset

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CustomDI
{
    public class ServiceProvider : IServiceProvider, ISupportRequiredService, IServiceScopeFactory
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly Dictionary<Type, object> _buildSingletonServices = new Dictionary<Type, object>();
        

        public ServiceProvider(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            _buildSingletonServices[typeof(IServiceProvider)] = this;
            _serviceCollection.AddSingleton<IServiceScopeFactory>(this);
        }

        public object? GetService(Type serviceType)
        {
            return null;
        }
        public object GetRequiredService(Type serviceType)
        {
            return GetServiceCore(serviceType);
        }

        public T GetService<T>(Type serviceType)
        {
            return (T)GetServiceCore(serviceType);
        }

        public IServiceScope CreateScope()
        {
            return new ServiceScope(this);
        }

        internal object GetServiceScoped(Type serviceType, ServiceScope scope)
        {
            return GetServiceCore(serviceType, scope);
        }

        private object GetServiceCore(Type service, ServiceScope scope = null)
        {
            var serviceDescriptor = _serviceCollection.FirstOrDefault(x => x.ServiceType == service);

            if (serviceDescriptor == null) throw new Exception("Service not found");

            var lifeTime = serviceDescriptor.Lifetime;

            if (lifeTime == ServiceLifetime.Singleton && _buildSingletonServices.ContainsKey(service)) return _buildSingletonServices[service];
            if (lifeTime == ServiceLifetime.Singleton && serviceDescriptor.ImplementationInstance != null) return serviceDescriptor.ImplementationInstance;
            if (lifeTime == ServiceLifetime.Scoped && scope?.BuildScopedServices == null) return new Exception("Cannot create scoped service without scope");
            if (lifeTime == ServiceLifetime.Scoped && scope.BuildScopedServices.ContainsKey(service)) return scope.BuildScopedServices[service];

            Type serviceType = serviceDescriptor.ImplementationType != null ? serviceDescriptor.ImplementationType : serviceDescriptor.ServiceType;

            var constructor = serviceType.GetConstructors().First();
            var constructorParameters = constructor.GetParameters();
            var parametersInstances = constructorParameters.Select(GetParameter).ToArray();

            //object serviceObject = CreateInstance(constructor, parametersInstances);
            object serviceObject = CreateInstance(serviceType, parametersInstances);

            switch (lifeTime)
            {
                case ServiceLifetime.Singleton:
                    _buildSingletonServices.Add(service, serviceObject);
                    break;
                case ServiceLifetime.Scoped:
                    serviceObject = scope.BuildScopedServices.GetOrAdd(service, serviceObject);
                    break;
                case ServiceLifetime.Transient:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            return serviceObject;
        }

        private static object CreateInstance(ConstructorInfo constructorInfo, object[] parametersInstances)
        {
            return constructorInfo.Invoke(
                BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
                null,
                parametersInstances,
                CultureInfo.CurrentCulture);
        }

        private static object CreateInstance(Type serviceType, object[] parametersInstances)
        {
            //serviceObject = Activator.CreateInstance(serviceType, parametersInstances);
            return Activator.CreateInstance(serviceType,
                BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
                null,
                parametersInstances,
                CultureInfo.CurrentCulture);
        }

        private object GetParameter(ParameterInfo parameter)
        {
            if (parameter.IsOptional && parameter.HasDefaultValue)
            {
                return Type.Missing;
            }
            else
            {
                return GetServiceCore(parameter.ParameterType);
            }
        }

    }
}