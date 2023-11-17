using FluentValidation;
using TrafficSimulation.Application.Vehicles;
using TrafficSimulation.Domain.Drivers;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Test.Vehicles
{
    public class MoveVehicleCommandValidatorTest : RequestValidatorTestBase<MoveVehicleCommandValidator, MoveVehicleCommand, Result<Vehicle>>
    {
        [Theory]
        [InlineData(-1, 0, 65, 2, 1, 55)]
        [InlineData(0, -1, 65, 2, 1, 55)]
        [InlineData(10, 0, -5, 2, 1, 55)]
        [InlineData(10, -1, 65, -1, 1, 55)]
        [InlineData(10, -1, 65, -1, 0, 55)]
        [InlineData(10, -1, 65, -1, 1, -55)]
        public async Task ShouldThrowExceptionForInvalidInput(int speed, int lane, int desiredSpeed, double followingInterval, int lanes, int speedLimit)
        {
            await Assert.ThrowsAsync<ValidationException>(async () => await ValidationBehavior.Handle(
                new MoveVehicleCommand
                {
                    Vehicle = new Vehicle
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
                    },
                    Road = new Domain.Roads.Road
                    {
                        Lanes = lanes,
                        SpeedLimit = speedLimit
                    }
                }, () => null, CancellationToken.None));
        }
    }
}
