using FluentValidation;
using TrafficSimulation.Domain.Roads;

namespace TrafficSimulation.Application.Roads
{
    public class RoadValidator : AbstractValidator<Road>
    {
        public RoadValidator()
        {
            RuleFor(x => x.SpeedLimit).GreaterThan(10).LessThanOrEqualTo(80);
            RuleFor(x => x.Lanes).GreaterThan(0).LessThanOrEqualTo(7);
        }
    }
}
