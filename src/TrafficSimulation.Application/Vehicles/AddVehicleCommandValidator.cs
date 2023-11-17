using FluentValidation;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Vehicles
{
    public class AddVehicleCommandValidator : AbstractValidator<AddVehiclesCommand>
    {
        public AddVehicleCommandValidator()
        {
            RuleFor(x => x.Vehicles).NotEmpty();
            RuleForEach(x => x.Vehicles).SetValidator(new VehicleValidator());
        }
    }
}
