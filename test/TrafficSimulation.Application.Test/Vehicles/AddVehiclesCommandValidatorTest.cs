using FluentValidation;
using TrafficSimulation.Application.Vehicles;
using TrafficSimulation.Domain.Drivers;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Test.Vehicles
{
    public class AddVehiclesCommandValidatorTest : RequestValidatorTestBase<AddVehicleCommandValidator, AddVehiclesCommand, Result<IEnumerable<Vehicle>>>
    {
        [Theory]
        [InlineData(-1, 0, 65, 2)]
        [InlineData(0, -1, 65, 2)]
        [InlineData(10, 0, -5, 2)]
        [InlineData(10, -1, 65, -1)]
        public async Task ShouldThrowExceptionForInvalidInput(int speed, int lane, int desiredSpeed, double followingInterval)
        {
            await Assert.ThrowsAsync<ValidationException>(async () => await ValidationBehavior.Handle(
                new AddVehiclesCommand
                {
                    Vehicles = new List<Vehicle>
                    {
                        new Vehicle
                        {
                            Speed = speed,
                            Position = new VehiclePosition
                            {
                                LaneNumber = lane
                            },
                            Driver = new Driver
                            {
                                FollowingInterval = followingInterval,
                                DesiredSpeed = desiredSpeed
                            }
                        }
                    }
                }, () => null, CancellationToken.None));
        }
    }
}
