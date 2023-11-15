using FluentValidation;

namespace TrafficSimulation.Application.Roads
{
    public class AddRoadCommandValidator : AbstractValidator<AddRoadCommand>
    {
        public AddRoadCommandValidator()
        {
            RuleFor(x => x.Road).NotNull();
            RuleFor(x => x.Road.SpeedLimit).GreaterThan(10).LessThanOrEqualTo(80);
            RuleFor(x => x.Road.Lanes).GreaterThan(0).LessThanOrEqualTo(7);
        }
    }
}
