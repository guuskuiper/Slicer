// unset

using FluentValidation;
using Slicer.Options;
using System.IO;
using System.Linq;

namespace Slicer.Validators
{
    public class SlicerServiceOptionsValidator : AbstractValidator<SlicerServiceOptions>
    {
        public SlicerServiceOptionsValidator()
        {
            RuleFor(options => options.InputFilePath)
                .NotEmpty()
                .Must(File.Exists).WithMessage("Input file does not exist");
            RuleFor(options => options.OutputFilePath)
                .NotEmpty()
                .Must(NotContainInvalidPathChars).WithMessage("Output filename contains invalid character(s)");
        }

        private bool NotContainInvalidPathChars(string input)
        {
            foreach (char c in input)
            {
                if (Path.GetInvalidPathChars().Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}