// unset

using FluentValidation;
using FluentValidation.Results;
using Slicer.Models;
using Slicer.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Validators
{
    public class ValidateAll
    {
        private readonly IValidator<SlicerServiceOptions> _slicerOptionsValidator;
        private readonly IValidator<Project> _projectValidator;
        private readonly Project _project;

        public ValidateAll(IValidator<SlicerServiceOptions> slicerOptionsValidator, IValidator<Project> projectValidator, Project project)
        {
            _slicerOptionsValidator = slicerOptionsValidator;
            _projectValidator = projectValidator;
            _project = project;
        }


        public string Validate(SlicerServiceOptions options)
        {
            var slicerOptionsResult = _slicerOptionsValidator.Validate(options);
            var projectResult = _projectValidator.Validate(_project);

            StringBuilder errorBuilder = new StringBuilder();

            if (!slicerOptionsResult.IsValid)
            {
                foreach (ValidationFailure error in slicerOptionsResult.Errors)
                {
                    errorBuilder.AppendLine(error.ErrorMessage);
                }
            }

            if (!projectResult.IsValid)
            {
                foreach (ValidationFailure error in projectResult.Errors)
                {
                    errorBuilder.AppendLine(error.ErrorMessage);
                }
            }

            return errorBuilder.ToString();
        }
    }
}