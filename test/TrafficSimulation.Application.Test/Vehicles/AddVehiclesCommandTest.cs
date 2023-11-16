using FluentAssertions;
using Moq;
using TrafficSimulation.Application.Vehicles;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Test.Vehicles
{
    public class AddVehiclesCommandTest : RequestTestBase<AddVehiclesCommandHandler>
    {
        private readonly Mock<IVehicleService> mockVehicleService = new();

        public AddVehiclesCommandTest()
        {
            handler = new AddVehiclesCommandHandler(mockVehicleService.Object, mockLogger.Object);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, false)]
        [InlineData(1, 0, 1, 1, true)]
        [InlineData(1, 0, 10, 1, true)]
        [InlineData(1, 0, 10, 0, true)]
        public async Task ShouldReturnFailureIfVehiclesInSamePosition(int position1, int lane1, int position2, int lane2, bool succeeded)
        {
            // Given
            var vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LaneNumber = lane1,
                    Position = position1,
                    Speed = 0
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LaneNumber = lane2,
                    Position = position2,
                    Speed = 0
                }
            };

            // When
            var result = await handler.Handle(new AddVehiclesCommand
            {
                Vehicles = vehicles
            }, CancellationToken.None);

            // Then
            result.Succeeded.Should().Be(succeeded);
        }
    }
}
