using FluentValidation;

namespace TrafficSimulation.Application.Roads
{
    public class AddRoadCommandValidator : AbstractValidator<AddRoadCommand>
    {
        public AddRoadCommandValidator()
        {
            RuleFor(x => x.Road).NotNull();
            RuleFor(x => x.Road).SetValidator(new RoadValidator());
        }
    }
}
