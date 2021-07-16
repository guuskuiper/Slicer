// unset

using ClipperLib;
using FluentAssertions;
using Slicer.Models;
using Slicer.Slicer.Clipper;
using Slicer.Slicer.Fill;
using Slicer.Slicer.Fill.Patterns;
using Slicer.Slicer.PolygonOperations;
using NSubstitute;
using Xunit;

namespace SlicerTests.Slicer.Fill
{
    public class FillerTests
    {
        private readonly IConcentric _concentric = Substitute.For<IConcentric>();
        private readonly IPatternFiller _patternFiller = Substitute.For<IPatternFiller>();
        private readonly IPatternFactory _patternFactory = Substitute.For<IPatternFactory>();
        private readonly Project _project = new Project();
        
        private readonly IFiller _filler;

        public FillerTests()
        {
            _filler = new Filler(_project, _concentric, _patternFiller, _patternFactory);
        }
        
        [Fact]
        public void FillPattern()
        {
            // Arrange
            var contour = new Polygons(CreatePolygon.SquarePoly(10_000));
            var layer = new Layer() {Contour = contour, Height = 0.2, Paths = new Polygons(), Thickness = 0.2};
            var patternArea = new Polygons(CreatePolygon.SquarePoly(9_000));

            var pattern = new ParallelPatternGenerator().CreatePattern(_project, Bounds.GetBounds(patternArea));
            _patternFactory.CreatePattern(_project, Arg.Any<IntRect>()).Returns(pattern);

            var concentricPaths = new Polygons(CreatePolygon.SquarePoly(9_500));
            var fillResult = new FillResult(concentricPaths, patternArea);
            _concentric.ConcentricFill(Arg.Any<Polygons>(), Arg.Any<double>(), 1).Returns(fillResult);

            var lines = new Polygons(
                new Polygon() {new IntPoint(8_500, -9_000), new IntPoint(8_500, 9_000)},
                new Polygon() {new IntPoint(7_500, -9_000), new IntPoint(7_500, 9_000)}
                );

            _patternFiller.Fill(Arg.Any<Polygons>(), pattern).Returns(lines);
            
            // Act
            _filler.Fill(layer);
            
            // Assert 
            layer.Paths.Should().HaveCount(lines.Count + concentricPaths.Count)
                .And.SatisfyRespectively(
                    x0 => x0.Should().BeSubsetOf(concentricPaths[0]),
                    x1 => x1.Should().BeSubsetOf(lines[0]),
                    x2 => x2.Should().BeSubsetOf(lines[1])
                );
        }
    }
}