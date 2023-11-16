namespace TrafficSimulation.Domain.Vehicles
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public int LaneNumber { get; set; }

        public int Speed { get; set; }

        public int Position { get; set; }

        public override string ToString()
        {
            return $"Lane: {LaneNumber}, Position: {Position} Speed: {Speed}";
        }
    }
}
