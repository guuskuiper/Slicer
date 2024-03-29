﻿// unset

using ClipperLib;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slicer.Middleware;
using Slicer.Models;
using Slicer.Services;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Clipper.Clipper1;
using Slicer.Slicer.Fill;
using Slicer.Slicer.Fill.Patterns;
using Slicer.Slicer.Input;
using Slicer.Slicer.Output;
using Slicer.Slicer.Slice;
using Slicer.Slicer.Sort;
using Slicer.Utils;
using Slicer.Validators;

namespace Slicer
{
    public static class SlicerServices
    {
        public static IServiceCollection AddSlicerServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddTransient<ISliceService, SliceService>();
            services.AddTransient<ISlicerStageResponse, STLStage>();
            services.AddTransient<ISlicerStageResponse, LayerStage>();
            services.AddTransient<ISlicerStageResponse, FillStage>();
            services.AddTransient<ISlicerStageResponse, SortingStage>();
            services.AddTransient<ISlicerStageResponse, GcodeStage>();
            services.AddTransient<ISliceService, PipelinedSliceService>();
            services.AddTransient<ISlicerStage, STLStage>();
            services.AddTransient<ISlicerStage, LayerStage>();
            services.AddTransient<ISlicerStage, FillStage>();
            services.AddTransient<ISlicerStage, SortingStage>();
            services.AddTransient<ISlicerStage, GcodeStage>();
            services.AddTransient<IFileIO, FileIO>();
            services.AddTransient<ILayers, Layers>();
            services.AddTransient<IFiller, Filler>();
            services.AddTransient<IPatternFiller, PatternFiller>();
            services.AddTransient<IPatternGenerator, ParallelPatternGenerator>();
            services.AddSingleton<IPatternFactory, PatternFactory>();
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
            services.AddTransient<ValidateAll>();
            services.AddValidatorsFromAssembly(typeof(SlicerServices).Assembly, ServiceLifetime.Transient);
            return services;
        }
    }
}