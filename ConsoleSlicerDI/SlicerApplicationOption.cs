﻿using CommandLine;

namespace ConsoleSlicerDI
{
    public class SlicerApplicationOption
    {
        [Option('o', "output", Required = false, HelpText = "Provide an output file path.")]
        public string Outfile { get; init; } = "gcode.gcode";

        [Option('i', "input", Required = false, HelpText = "Provide an STL file path.")]
        public string Infile { get; init; } = "Solid cube 10mm.stl";

        [Option('p', "parallel", Required = false, HelpText = "Slice in parallel.")]
        public bool Parallel { get; init; } = true;
    }
}