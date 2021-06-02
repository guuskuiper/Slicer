// unset

using Slicer.Models;

namespace Slicer.Slicer.Fill
{
    public class Filler : IFiller
    {
        private readonly Project _project;
        private readonly IConcentric _concentric;

        public Filler(Project project, IConcentric concentric)
        {
            _project = project;
            _concentric = concentric;
        }

        public void Fill(Layer layer)
        {
            layer.Paths.AddRange(_concentric.ConcentricFill(layer.Contour, _project.setting.LineWidth));
        }

        public void FillOutside(Layer layer)
        {
            layer.Paths.AddRange(_concentric.ConcentricOutside(layer.Contour, _project.setting.LineWidth, _project.setting.BrimCount));
        }
    }
}