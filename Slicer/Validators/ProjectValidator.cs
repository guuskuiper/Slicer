// unset

using FluentValidation;
using Slicer.Models;

namespace Slicer.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Settings).SetValidator(new SettingsValidator());
        }
    }
}