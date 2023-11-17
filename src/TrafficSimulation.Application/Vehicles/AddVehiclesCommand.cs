using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TrafficSimulation.Application.Extensions;
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
                var vehicles = FilterOverlappingVehicles(request.Vehicles);
                vehicleService.Add(vehicles);
                return Task.FromResult(Result<IEnumerable<Vehicle>>.Success(vehicles));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding vehicles");
                return Task.FromResult(Result<IEnumerable<Vehicle>>.Failure(ex));
            }
        }

        private IEnumerable<Vehicle> FilterOverlappingVehicles(IEnumerable<Vehicle> vehicles)
        {
            var filteredVehicles = vehicles.Where(v => !v.HasCollidedWithAnotherVehicle(vehicles)).ToList();
            var filteredOutVehicleIds = new List<Guid>();

            var collisions = vehicles.Where(v => v.HasCollidedWithAnotherVehicle(vehicles));
            foreach (var vehicle in collisions)
            {
                if (filteredOutVehicleIds.Contains(vehicle.Id))
                {
                    continue;
                }

                filteredVehicles.Add(vehicle);
                filteredOutVehicleIds.AddRange(vehicle.GetVehicleCollisions(collisions).Select(v => v.Id));
            }

            return filteredVehicles.OrderByDescending(v => v.Position.Front);
        }
    }
}
