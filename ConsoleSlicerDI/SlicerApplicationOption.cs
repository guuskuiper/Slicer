using CommandLine;

namespace ConsoleSlicerDI
{
    public class SlicerApplicationOption
    {
        [Option('o', "output", Required = false, HelpText = "Provide an output file path.")]
        public string Outfile { get; init; } = "gcode.gcode";

        [Option('i', "input", Required = false, HelpText = "Provide an STL file path.")]
        public string Infile { get; init; } = "Solid cilinder 20mm.stl";

        [Option('p', "parallel", Required = false, HelpText = "Slice in parallel.")]
        public bool Parallel { get; init; } = true;

        [Option('m', "mediatr", Required = false, HelpText = "Use mediatR.")]
        public bool MediatR { get; init; } = false;
    }
}