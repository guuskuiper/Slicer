// unset

using MediatR;
using Slicer;
using Slicer.Services;
using SlicerMediatR.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace SlicerMediatR.Handlers
{
    public class SlicerHandler : IRequestHandler<SliceCommand, string>
    {
        private readonly ISliceService _sliceService;

        public SlicerHandler(ISliceService services)
        {
            _sliceService = services;
        }

        public Task<string> Handle(SliceCommand request, CancellationToken cancellationToken)
        {
            return _sliceService.Slice(request.Options);
        }
    }
}