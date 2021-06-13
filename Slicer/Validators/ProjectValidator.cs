// unset

using FluentValidation;
using Slicer.Models;

namespace Slicer.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.setting).SetValidator(new SettingsValidator());
        }
    }
}