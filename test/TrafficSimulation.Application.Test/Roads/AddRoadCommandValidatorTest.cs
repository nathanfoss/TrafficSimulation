using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSimulation.Application.Roads;
using TrafficSimulation.Domain.Roads;

namespace TrafficSimulation.Application.Test.Roads
{
    public class AddRoadCommandValidatorTest : RequestValidatorTestBase<AddRoadCommandValidator, AddRoadCommand, Result<Road>>
    {
        [Theory]
        [InlineData(-1, 1)]
        [InlineData(5, 5)]
        [InlineData(500, 5)]
        [InlineData(20, 20)]
        [InlineData(20, -1)]
        public async Task ShouldThrowExceptionOnInvalidInput(int speedLimit, int lanes)
        {
            await Assert.ThrowsAsync<ValidationException>(async () => await ValidationBehavior.Handle(
                new AddRoadCommand
                {
                    Road = new Road
                    {
                        SpeedLimit = speedLimit,
                        Lanes = lanes
                    }
                }, () => null, CancellationToken.None));
        }
    }
}
