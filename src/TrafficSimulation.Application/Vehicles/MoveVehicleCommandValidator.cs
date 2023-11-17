using FluentValidation;
using TrafficSimulation.Application.Roads;

namespace TrafficSimulation.Application.Vehicles
{
    public class MoveVehicleCommandValidator : AbstractValidator<MoveVehicleCommand>
    {
        public MoveVehicleCommandValidator()
        {
            RuleFor(x => x.Road).NotNull();
            RuleFor(x => x.Road).SetValidator(new RoadValidator());

            RuleFor(x => x.Vehicle).NotNull();
            RuleFor(x => x.Vehicle).SetValidator(new VehicleValidator());
        }
    }
}
