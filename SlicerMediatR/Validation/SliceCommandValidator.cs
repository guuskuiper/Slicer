// unset

using FluentValidation;
using Slicer.Validators;
using SlicerMediatR.Commands;

namespace SlicerMediatR.Validation
{
    public class SliceCommandValidator : AbstractValidator<SliceCommand>
    {
        public SliceCommandValidator()
        {
            RuleFor(x => x.Options).SetValidator(new SlicerServiceOptionsValidator());
        }
    }
}