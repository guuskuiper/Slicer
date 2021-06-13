// unset

namespace Slicer.Options
{
    public record SlicerServiceOptions
    {
        public string InputFilePath { get; init; }
        public string OutputFilePath { get; init; }
        public bool Parallel { get; init; } = true;
    }
}