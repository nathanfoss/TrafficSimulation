using FluentValidation;
using TrafficSimulation.Application.Vehicles;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Test.Vehicles
{
    public class AddVehiclesCommandValidatorTest : RequestValidatorTestBase<AddVehicleCommandValidator, AddVehiclesCommand, Result<IEnumerable<Vehicle>>>
    {
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public async Task ShouldThrowExceptionForInvalidInput(int speed, int lane)
        {
            await Assert.ThrowsAsync<ValidationException>(async () => await ValidationBehavior.Handle(
                new AddVehiclesCommand
                {
                    Vehicles = new List<Vehicle>
                    {
                        new Vehicle
                        {
                            Speed = speed,
                            LaneNumber = lane
                        }
                    }
                }, () => null, CancellationToken.None));
        }
    }
}
