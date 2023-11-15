using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSimulation.Application.Roads;
using TrafficSimulation.Domain.Roads;

namespace TrafficSimulation.Application.Test.Roads
{
    public class AddRoadCommandTest : RequestTestBase<AddRoadCommandHandler>
    {
        private readonly Mock<IRoadService> mockRoadService = new();

        public AddRoadCommandTest()
        {
            handler = new AddRoadCommandHandler(mockRoadService.Object, mockLogger.Object);
        }

        [Fact]
        public async Task ShouldSuccess()
        {
            // Given

            // When
            var result = await handler.Handle(new AddRoadCommand(), CancellationToken.None);

            // Then
            result.Succeeded.Should().BeTrue();
        }
    }
}
