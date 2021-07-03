# Slicer
My own slicer implemented using Dependency Injection in .Net 5.

# How to build / run
Install the .NET 5 SDK: https://dotnet.microsoft.com/download/dotnet/5.0

Build:
```dotnet build```

Run:
```dotnet run -p ConsoleSlicerDI -- <options>```

Options:
```
  -o, --output      Provide an output file path.

  -i, --input       Provide an STL file path.

  -p, --parallel    Slice in parallel.

  -m, --mediatr     Use mediatR.

  --help            Display this help screen.

  --version         Display version information.
```
# Solution structure

## Benchmark
Compare different implementation using BenchmarkDotNet.

## ConsoleSlicerDI
Console application that calls the Slicer.

## CustomDI
How does Dependency Injection work? Find out by creating you own framework implementatie.

## Slicer
Core of the slicing algorithm:
- Read 3D model
- Create layers
- Fill layers
- Add additional structures (brim)
- Sort layers
- Create Gcode

## SlicerMediatR
Use MediatR and define a Slice command/request and the corresponsing handler.

## SlicerTests
Unit tests for the Slicer project.
