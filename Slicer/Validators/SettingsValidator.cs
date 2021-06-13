// unset

using FluentValidation;

namespace Slicer.Validators
{
    public class SettingsValidator : AbstractValidator<Settings.Settings>
    {
        public SettingsValidator()
        {
            RuleFor(x => x.BrimCount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LayerHeight).ExclusiveBetween(0, 0.5f);
            RuleFor(x => x.PrintSpeed).InclusiveBetween(1, 100);
            RuleFor(x => x.TravelSpeed).InclusiveBetween(1, 100);
            RuleFor(x => x.LineWidth).InclusiveBetween(10, 1000.0);
        }
    }
}