using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ConsoleSlicerDI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogServices(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().
                ReadFrom.Configuration(configuration)
                .CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            return services.AddSingleton(Log.Logger);
        }
    }
}