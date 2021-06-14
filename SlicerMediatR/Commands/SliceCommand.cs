// unset

using MediatR;
using Slicer.Options;

namespace SlicerMediatR.Commands
{
    public record SliceCommand(SlicerServiceOptions Options) : IRequest<string>;
}