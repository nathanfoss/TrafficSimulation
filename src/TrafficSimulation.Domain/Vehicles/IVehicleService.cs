namespace TrafficSimulation.Domain.Vehicles
{
    public interface IVehicleService
    {
        void Add(IEnumerable<Vehicle> vehicles);

        Vehicle Get(Guid vehicleId);

        IEnumerable<Vehicle> GetNearby(Vehicle vehicle);

        void Update(Vehicle vehicle);
    }
}
