// unset

using System;
using System.Collections.Generic;

namespace Slicer.Utils
{
    public interface IParallelScope
    {
        void Parallelize<T, TS>(IEnumerable<T> items, Action<T, TS> action);
    }
}