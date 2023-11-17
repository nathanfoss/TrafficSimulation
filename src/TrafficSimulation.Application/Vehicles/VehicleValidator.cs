using FluentValidation;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Vehicles
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        public VehicleValidator()
        {
            RuleFor(x => x.Speed).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Position).NotNull();
            RuleFor(x => x.VehicleType).NotNull();
            RuleFor(x => x.Driver).NotNull();
            RuleFor(x => x.Position.LaneNumber).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Driver.FollowingInterval).GreaterThan(0);
            RuleFor(x => x.Driver.DesiredSpeed).GreaterThan(0);
        }
    }
}
