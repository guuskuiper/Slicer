# Slicer
My own slicer implemented using Dependency Injection in .Net 5.

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
