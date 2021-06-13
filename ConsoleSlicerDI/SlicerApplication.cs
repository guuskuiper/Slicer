using System.Threading.Tasks;
using CommandLine;
using FluentValidation;
using MediatR;
using Slicer.Options;
using Slicer.Services;
using Slicer.Validators;
using SlicerMediatR.Commands;

namespace ConsoleSlicerDI
{
    public class SlicerApplication
    {
        private readonly ISliceService _sliceService;
        private readonly IMediator _mediator;
        private readonly ValidateAll _validator;

        public SlicerApplication(ISliceService sliceService, IMediator mediator, ValidateAll validator)
        {
            _sliceService = sliceService;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task RunAsync(string[] args)
        {
            await Parser.Default
                .ParseArguments<SlicerApplicationOption>(args)
                .WithParsedAsync(options =>
                {
                    if (options.MediatR)
                    {
                        return RunMediatR(options);
                    }
                    else
                    {
                        return RunDefault(options);
                    }
                });
        }

        private Task<string> RunDefault(SlicerApplicationOption options)
        {
            SlicerServiceOptions slicerServiceOptions = SlicerServiceOptions(options);

            var validatorResult = _validator.Validate(slicerServiceOptions);
            if (!string.IsNullOrEmpty(validatorResult))
            {
                throw new ValidationException(validatorResult);
            }

            return _sliceService.Slice(slicerServiceOptions);
        }

        private Task<string> RunMediatR(SlicerApplicationOption options)
        {
            SlicerServiceOptions slicerServiceOptions = SlicerServiceOptions(options);
            return _mediator.Send(new SliceCommand(slicerServiceOptions));
        }

        private static SlicerServiceOptions SlicerServiceOptions(SlicerApplicationOption options)
        {
            return new SlicerServiceOptions()
            {
                InputFilePath = options.Infile,
                OutputFilePath = options.Outfile,
                Parallel = options.Parallel
            };
        }
    }
}