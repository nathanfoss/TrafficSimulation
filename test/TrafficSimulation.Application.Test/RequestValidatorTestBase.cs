using FluentValidation;
using MediatR;

namespace TrafficSimulation.Application.Test
{
    public class RequestValidatorTestBase<TValidator, TRequest, TResponse> where TValidator : AbstractValidator<TRequest>, new()
    where TRequest : IRequest<TResponse>
    {
        public readonly TValidator Validator;
        public readonly RequestValidationBehavior<TRequest, TResponse> ValidationBehavior;

        public RequestValidatorTestBase()
        {
            Validator = new TValidator();
            ValidationBehavior = new RequestValidationBehavior<TRequest, TResponse>(new List<IValidator<TRequest>> { Validator });
        }
    }
}
