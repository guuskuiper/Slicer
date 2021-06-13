// unset

using FluentValidation;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlicerMediatR.PipelineBehaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //pre
            var context = new ValidationContext<TRequest>(request);

            var errors = _validators.Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where( x => x!= null)
                .ToList();

            if (errors.Any())
            {
                _logger.Error(string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage)));
                return default;
                //throw new ValidationException(errors);
            }

            return await next();
            //post
        }
    }
}