namespace TrafficSimulation.Domain.Vehicles
{
    public class VehiclePosition
    {
        public int Back { get; set; }

        public int Front { get; set; }

        public int LaneNumber { get; set; }

        public override string ToString()
        {
            return $"Lane: {LaneNumber}, Front: {Front} Back: {Back}";
        }
    }
}
