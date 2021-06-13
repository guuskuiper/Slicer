using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Slicer;
using SlicerMediatR;

namespace ConsoleSlicerDI
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();
            var serviceProvider = BuildServiceProvider(configuration);

            var logger = serviceProvider.GetRequiredService<ILogger>();
            var app = serviceProvider.GetRequiredService<SlicerApplication>();

            try
            {
                await app.RunAsync(args);
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Unhandled exception {message}", e.Message);
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<SlicerApplication>();
            services.AddSlicerServices(configuration);
            services.AddMediatRServices(configuration);
            services.AddSerilogServices(configuration);
        }

        private static ServiceProvider BuildServiceProvider(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
            return serviceProvider;
        }

        private static IConfigurationRoot BuildConfiguration() => 
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
    }
}
