// unset

using MediatR;
using Slicer.Options;

namespace SlicerMediatR.Commands
{
    public class SliceCommand : IRequest<string>
    {
        public SlicerServiceOptions Options { get; }

        public SliceCommand(SlicerServiceOptions options)
        {
            Options = options;
        }
    }
}