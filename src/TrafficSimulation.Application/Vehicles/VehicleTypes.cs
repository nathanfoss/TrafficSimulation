using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Vehicles
{
    public static class VehicleTypes
    {
        public static VehicleType Compact = new()
        {
            Id = VehicleTypeEnum.Compact,
            Size = 13
        };

        public static VehicleType Sedan = new()
        {
            Id = VehicleTypeEnum.Sedan,
            Size = 15
        };

        public static VehicleType Minivan = new()
        {
            Id = VehicleTypeEnum.Minivan,
            Size = 17
        };

        public static VehicleType Semi = new()
        {
            Id = VehicleTypeEnum.Semi,
            Size = 60
        };

        public static VehicleType Pickup = new()
        {
            Id = VehicleTypeEnum.Pickup,
            Size = 18
        };

        public static VehicleType Motorcycle = new()
        {
            Id = VehicleTypeEnum.Motorcycle,
            Size = 8
        };

        public static List<VehicleType> All = new()
        {
            Compact,
            Sedan,
            Pickup,
            Minivan,
            Semi,
            Motorcycle
        };
    }
}
