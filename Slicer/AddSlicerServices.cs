// unset

using ClipperLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slicer.Models;
using Slicer.Services;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Fill;
using Slicer.Slicer.Input;
using Slicer.Slicer.Output;
using Slicer.Slicer.Slice;
using Slicer.Slicer.Sort;
using Slicer.Utils;

namespace Slicer
{
    public static class SlicerServices
    {
        public static IServiceCollection AddSlicerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISliceService, SliceService>();
            services.AddTransient<IFileIO, FileIO>();
            services.AddTransient<ILayers, Layers>();
            services.AddTransient<IFiller, Filler>();
            services.AddTransient<ISort, Sort>();
            services.AddSingleton<Project>();
            services.AddTransient<GcodeState>();
            services.AddTransient<IGcodeCommands, GcodeCommands>();
            services.AddTransient<IGcode, Gcode>();
            services.AddTransient<IConcentric, Concentric>();
            services.AddTransient<IOffset, Offset>();
            services.AddTransient<ClipperOffset>();
            services.AddTransient<IClip, Clip>();
            services.AddTransient<Clipper>();
            services.AddTransient<STLConverter>();
            services.AddTransient<Intersection>();
            services.AddTransient<IParallelScope, ParallelScopeLocal>();
            services.AddTransient<ISliceLayer, SliceLayer>();
            return services;
        }
    }
}