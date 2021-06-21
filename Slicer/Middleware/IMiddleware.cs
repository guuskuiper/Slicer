﻿// unset

using Slicer.Models;
using System;
using System.Threading.Tasks;

namespace Slicer.Middleware
{
    public interface IMiddleware<TRequest, TResponse>
    {
        public Task<TResponse> Execute(TRequest request, Func<TRequest, Task<TResponse>> next);
    }
    
    public delegate Task NextDelegate();
    
    public interface IMiddleware<in TRequest>
    {
        public Task Execute(TRequest request, NextDelegate next);
    }
}