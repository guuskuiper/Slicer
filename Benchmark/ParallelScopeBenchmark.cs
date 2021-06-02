using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Slicer;
using Slicer.Models;
using Slicer.Settings;
using Slicer.Slicer.Input;
using Slicer.Slicer.Slice;
using Slicer.Utils;
using System.Collections.Generic;

namespace Benchmark
{
    [MemoryDiagnoser]
    public class ParallelScopeBenchmark
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly STL _stl;
        private readonly Project _project;

        public ParallelScopeBenchmark()
        {
            var configuration = BuildConfiguration();
            var serviceProvider = BuildServiceProvider(configuration);

            _scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            _logger = serviceProvider.GetRequiredService<ILogger>();
            _project = serviceProvider.GetRequiredService<Project>();
            _stl = serviceProvider.GetRequiredService<STLConverter>().Read("Solid cilinder 20mm.stl");
            
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();
            return configuration;
        }

        private static ServiceProvider BuildServiceProvider(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();
            services.AddSingleton(Log.Logger);
            services.AddSlicerServices(configuration);
            var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
                {ValidateOnBuild = true, ValidateScopes = true});
            return serviceProvider;
        }

        [Benchmark]
        public List<Layer> Parallel()
        {
            IParallelScope parallel = new ParallelScope(_scopeFactory, _logger);
            ILayers layers = new Layers(_project, null, parallel);

            return layers.CreateLayers(_stl, true);
        }

        [Benchmark]
        public List<Layer> ParallelLocal()
        {
            IParallelScope parallel = new ParallelScopeLocal(_scopeFactory, _logger);
            ILayers layers = new Layers(_project, null, parallel);

            return layers.CreateLayers(_stl, true);
        }

        [Benchmark]
        public List<Layer> ParallelRange()
        {
            IParallelScope parallel = new ParallelScopeRange(_scopeFactory, _logger);
            ILayers layers = new Layers(_project, null, parallel);

            return layers.CreateLayers(_stl, true);
        }

        [Benchmark]
        public List<Layer> Single()
        {
            ISliceLayer sliceLayer = new SliceLayer(new Intersection());
            ILayers layers = new Layers(_project, sliceLayer, null);

            return layers.CreateLayers(_stl, false);
        }
    }
}