using TrafficSimulation.Domain.Roads;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Test.Vehicles
{
    public class TrafficExpectationModel
    {
        public Vehicle Vehicle { get; set; }

        public IEnumerable<Vehicle> NearbyVehicles { get; set; }

        public Road Road { get; set; }

        public int ExpectedSpeed { get; set; }

        public int ExpectedLane { get; set; }

        public int ExpectedFront { get; set; }
    }
}
