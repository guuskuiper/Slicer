// unset

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CustomDI
{
    public class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
        public IServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(this);
        }
    }
}