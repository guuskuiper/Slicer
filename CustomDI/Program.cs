using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Slicer;
using Slicer.Options;
using Slicer.Services;
using Slicer.Slicer.Slice;
using System;
using System.Threading.Tasks;

namespace CustomDI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();

            var serviceCollection = new ServiceCollection();
            AddSerilogServices(serviceCollection, configuration);
            serviceCollection.AddSlicerServices(configuration);
            serviceCollection.AddScoped<GenerateGUID>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger>();

            var slicer = serviceProvider.GetRequiredService<ISliceService>();

            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var guid1 = scope.ServiceProvider.GetService<GenerateGUID>();
                var guid2 = scope.ServiceProvider.GetService<GenerateGUID>();
            }

            try
            {
                await slicer.Slice(new SlicerServiceOptions("Solid cilinder 20mm.stl", "Custom.gcode", true));
            }
            catch (Exception e)
            {
                logger.Fatal(e.Message);
            }
        }

        public static IServiceCollection AddSerilogServices(IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().
                ReadFrom.Configuration(configuration)
                .CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            return services.AddSingleton(Log.Logger);
        }

        private static IConfigurationRoot BuildConfiguration() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
    }
}
