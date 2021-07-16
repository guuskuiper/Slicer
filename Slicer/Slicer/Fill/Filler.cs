// unset

using Slicer.Models;
using Slicer.Slicer.Fill.Patterns;
using Slicer.Slicer.PolygonOperations;
using System;

namespace Slicer.Slicer.Fill
{
    public class Filler : IFiller
    {
        private readonly Project _project;
        private readonly IConcentric _concentric;
        private readonly IPatternFiller _patternFiller;
        private readonly IPatternFactory _patternFactory;

        public Filler(Project project, IConcentric concentric, IPatternFiller patternFiller, IPatternFactory patternFactory)
        {
            _project = project;
            _concentric = concentric;
            _patternFiller = patternFiller;
            _patternFactory = patternFactory;
        }

        public void Fill(Layer layer)
        {
            var fillResult = _concentric.ConcentricFill(layer.Contour, -_project.Settings.LineWidth, _project.Settings.ConcentricPathCount);
            layer.Paths.AddRange(fillResult.Paths);
            
            var bounds = Bounds.GetBounds(fillResult.RemainingArea);
            var pattern = _patternFactory.CreatePattern(_project, bounds);

            layer.Paths.AddRange(_patternFiller.Fill(fillResult.RemainingArea, pattern));
        }

        public void FillOutside(Layer layer)
        {
            layer.Paths.AddRange(_concentric.ConcentricOutside(layer.Contour, _project.Settings.LineWidth, _project.Settings.BrimCount).Paths);
        }
    }
}