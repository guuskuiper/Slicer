// unset

using FluentAssertions;
using Slicer.Models;
using Slicer.Settings;
using Slicer.Slicer.Fill.Patterns;
using Xunit;

namespace SlicerTests.Slicer.Fill
{
    public class ParallelPatternTests
    {
        [Fact]
        public void ParallelPatternTest()
        {
            // Arrange
            IPatternGenerator parallelPatternGenerator = new ParallelPatternGenerator();
            Project project = new Project()
            {
                Settings = new Settings()
                {
                    LineWidth = 1000
                }
            };

            Rect bound = new (-10_000, 10_000, 10_000, -10_000);

            // Act
            var pattern = parallelPatternGenerator.CreatePattern(project, bound);

            // Assert 
            pattern.Polygons.Should().HaveCount(20);
        }
    }
}