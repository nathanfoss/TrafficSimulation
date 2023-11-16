namespace TrafficSimulation.Domain.Vehicles
{
    public class VehicleType
    {
        public VehicleTypeEnum Id { get; set; }

        public int Size { get; set; }

        public override string ToString()
        {
            return $"{Id}";
        }
    }

    public enum VehicleTypeEnum
    {
        Compact,
        Sedan,
        Semi,
        Minivan,
        Pickup,
        Motorcycle
    }
}
