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
        [InlineData(0, 0, 0, 0, 1, true)]
        [InlineData(0, 0, 10, 0, 1, true)]
        [InlineData(0, 0, -10, 0, 1, true)]
        [InlineData(1, 0, 1, 1, 2, true)]
        [InlineData(1, 0, 100, 1, 2, true)]
        [InlineData(1, 0, 100, 0, 2, true)]
        public async Task ShouldFilterOutVehiclesInSamePosition(int position1, int lane1, int position2, int lane2, int addedCount, bool succeeded)
        {
            // Given
            var vehicleType = VehicleTypes.Sedan;
            var vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    VehicleType = vehicleType,
                    Position = new VehiclePosition
                    {
                        Front = position1,
                        LaneNumber = lane1,
                        Back = position1 - vehicleType.Size
                    },
                    Speed = 0
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    VehicleType = vehicleType,
                    Position = new VehiclePosition
                    {
                        Front = position2,
                        LaneNumber = lane2,
                        Back = position2 - vehicleType.Size
                    },
                    Speed = 0
                }
            };

            // When
            var result = await handler.Handle(new AddVehiclesCommand
            {
                Vehicles = vehicles
            }, CancellationToken.None);

            // Then
            mockVehicleService.Verify(x => x.Add(It.Is<IEnumerable<Vehicle>>(l => l.Count() == addedCount)));
            result.Succeeded.Should().Be(succeeded);
        }
    }
}
