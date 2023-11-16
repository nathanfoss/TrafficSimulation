using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Infrastructure.Vehicles
{
    public class VehicleService : IVehicleService
    {
        private List<Vehicle> _vehicles;

        public VehicleService()
        {
            _vehicles = new List<Vehicle>();
        }

        public void Add(IEnumerable<Vehicle> vehicles)
        {
            _vehicles.AddRange(vehicles);
        }

        public Vehicle Get(Guid vehicleId)
        {
            return _vehicles.First(x => x.Id == vehicleId);
        }

        public IEnumerable<Vehicle> GetNearby(Vehicle vehicle)
        {
            var minPosition = vehicle.Position.Back - vehicle.Speed;
            var maxPosition = vehicle.Position.Front + vehicle.Speed;

            return _vehicles.Where(x => x.Id != vehicle.Id && x.Position.Front >= minPosition && x.Position.Back <= maxPosition);
        }

        public void Update(Vehicle vehicle)
        {
            var vehicleToUpdate = Get(vehicle.Id);
            vehicleToUpdate.Position = vehicle.Position;
            vehicleToUpdate.Speed = vehicle.Speed;
        }
    }
}
