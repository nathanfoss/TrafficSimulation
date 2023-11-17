using System.Drawing;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Extensions
{
    public static class VehicleExtensions
    {
        public static bool HasCollidedWithAnotherVehicle(this Vehicle vehicle, IEnumerable<Vehicle> vehicles)
        {
            var rect = new Rectangle(vehicle.Position.Back, vehicle.Position.LaneNumber, vehicle.VehicleType.Size, 1);
            return vehicles.Where(v => v.Id != vehicle.Id)
                .Any(v =>
                {
                    var rect2 = new Rectangle(v.Position.Back, v.Position.LaneNumber, v.VehicleType.Size, 1);
                    return rect.IntersectsWith(rect2);
                });
        }

        public static IEnumerable<Vehicle> GetVehicleCollisions(this Vehicle vehicle, IEnumerable<Vehicle> vehicles)
        {
            var rect = new Rectangle(vehicle.Position.Back, vehicle.Position.LaneNumber, vehicle.VehicleType.Size, 1);
            return vehicles.Where(v => v.Id != vehicle.Id)
                .Where(v =>
                {
                    var rect2 = new Rectangle(v.Position.Back, v.Position.LaneNumber, v.VehicleType.Size, 1);
                    return rect.IntersectsWith(rect2);
                });
        }
    }
}
