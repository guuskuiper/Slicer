// unset

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlicerMediatR.PipelineBehaviours;

namespace SlicerMediatR
{
    public static class MediatRServices
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(MediatRServices));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TimingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssembly(typeof(MediatRServices).Assembly, ServiceLifetime.Transient);
            return services;
        }
    }
}