// unset

using System;

namespace CustomDI
{
    public class GenerateGUID
    {
        public Guid Guid { get; } = Guid.NewGuid();
    }
}