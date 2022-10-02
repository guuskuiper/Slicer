// unset

using Slicer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Slicer.Slicer.Fill.Patterns
{
    public class PatternFactory : IPatternFactory
    {
        private readonly IReadOnlyDictionary<string, IPatternGenerator> _patternGenerators;

        public PatternFactory(IEnumerable<IPatternGenerator> patternGenerators)
        {
            _patternGenerators = patternGenerators.ToImmutableDictionary(x => x.PatternName, x => x);
        }

        public IPattern CreatePattern(Project project, Rect boundingBox)
        {
            var patternGenerator = _patternGenerators[project.Settings.PatternName] ?? _patternGenerators.Values.First();

            return patternGenerator.CreatePattern(project, boundingBox);
        }
    }
}