using TrafficSimulation.Domain.Drivers;

namespace TrafficSimulation.Domain.Vehicles
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public VehicleType VehicleType { get; set; }

        public int Speed { get; set; }

        public VehiclePosition Position { get; set; }

        public Driver Driver { get; set; }

        public int ComfortableMargin { get => (int)(Speed * Driver.FollowingInterval); }

        public override string ToString()
        {
            return $"Type: {VehicleType}, Position: {Position} Speed: {Speed}";
        }

        public void MoveForward(int distance)
        {
            Position.Front += distance;
            Position.Back += distance;
        }
    }
}
