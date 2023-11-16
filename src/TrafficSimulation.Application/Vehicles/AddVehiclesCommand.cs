using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Vehicles
{
    public class AddVehiclesCommand : IRequest<Result<IEnumerable<Vehicle>>>
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }

    public class AddVehiclesCommandHandler : IRequestHandler<AddVehiclesCommand, Result<IEnumerable<Vehicle>>>
    {
        private readonly IVehicleService vehicleService;

        private readonly ILogger<AddVehiclesCommandHandler> logger;

        public AddVehiclesCommandHandler(IVehicleService vehicleService, ILogger<AddVehiclesCommandHandler> logger)
        {
            this.vehicleService = vehicleService;
            this.logger = logger;
        }

        public Task<Result<IEnumerable<Vehicle>>> Handle(AddVehiclesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicles = request.Vehicles;
                ValidateVehiclePosition(vehicles);
                vehicleService.Add(vehicles);
                return Task.FromResult(Result<IEnumerable<Vehicle>>.Success(vehicles));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding vehicles");
                return Task.FromResult(Result<IEnumerable<Vehicle>>.Failure(ex));
            }
        }

        private void ValidateVehiclePosition(IEnumerable<Vehicle> vehicles)
        {
            var duplicates = vehicles.GroupBy(x => new { x.Position, x.LaneNumber })
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .ToList();
            if (duplicates.Any())
            {
                var duplicatePosition = duplicates.First();
                throw new ValidationException($"Duplicate vehicles found at position {duplicatePosition.Position} and lane {duplicatePosition.LaneNumber}");
            }
        }
    }
}
