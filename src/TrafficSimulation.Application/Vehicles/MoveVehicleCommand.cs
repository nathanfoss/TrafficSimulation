using MediatR;
using Microsoft.Extensions.Logging;
using TrafficSimulation.Domain.Roads;
using TrafficSimulation.Domain.Vehicles;

namespace TrafficSimulation.Application.Vehicles
{
    public class MoveVehicleCommand : IRequest<Result<Vehicle>>
    {
        public Vehicle Vehicle { get; set; }

        public Road Road { get; set; }
    }

    public class MoveVehicleCommandHandler : IRequestHandler<MoveVehicleCommand, Result<Vehicle>>
    {
        private readonly IVehicleService vehicleService;

        private readonly ILogger<MoveVehicleCommandHandler> logger;

        public MoveVehicleCommandHandler(IVehicleService vehicleService, ILogger<MoveVehicleCommandHandler> logger)
        {
            this.vehicleService = vehicleService;
            this.logger = logger;
        }

        public Task<Result<Vehicle>> Handle(MoveVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = request.Vehicle;
                var road = request.Road;

                var nearbyVehicles = vehicleService.GetNearby(vehicle);
                var vehicleAhead = GetClosestVehicleAhead(vehicle, nearbyVehicles, vehicle.Position.LaneNumber);

                if (vehicleAhead is null)
                {
                    var updated = RestoreSpeedAndMoveForward(vehicle);
                    return Task.FromResult(Result<Vehicle>.Success(updated));
                }

                var needsSpeedAdjustment = vehicleAhead.Position.Back <= vehicle.Position.Front + vehicle.Driver.DesiredSpeed * vehicle.Driver.FollowingInterval;

                if (needsSpeedAdjustment)
                {
                    var updated = AdjustToTraffic(vehicle, road, vehicleAhead, nearbyVehicles);
                    return Task.FromResult(Result<Vehicle>.Success(updated));
                }

                // TODO:

                // If not at full speed
                //      try to return to full speed
                // If not able to return to full speed
                //      try to change lanes
                // Move forward

                vehicleService.Update(vehicle);
                return Task.FromResult(Result<Vehicle>.Success(vehicle));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error moving vehicle {Vehicle}", request.Vehicle.Id);
                return Task.FromResult(Result<Vehicle>.Failure(ex));
            }
        }

        private Vehicle AdjustToTraffic(Vehicle vehicle, Road road, Vehicle vehicleAhead, IEnumerable<Vehicle> nearbyVehicles)
        {
            if (CanMoveLeft(vehicle, road))
            {
                var laneToCheck = vehicle.Position.LaneNumber + 1;
                return AttemptToMerge(vehicle, vehicleAhead, nearbyVehicles, laneToCheck);
            }

            if (CanMoveRight(vehicle))
            {
                var laneToCheck = vehicle.Position.LaneNumber - 1;
                return AttemptToMerge(vehicle, vehicleAhead, nearbyVehicles, laneToCheck);
            }

            var speed = vehicleAhead.Speed;
            return AdjustSpeed(vehicle, speed);
        }

        private Vehicle AttemptToMerge(Vehicle vehicle, Vehicle vehicleAhead, IEnumerable<Vehicle> nearbyVehicles, int laneToCheck)
        {
            var closestVehicleAhead = GetClosestVehicleAhead(vehicle, nearbyVehicles, laneToCheck);
            var closestVehicleBehind = GetClosestVehicleBehind(vehicle, nearbyVehicles, laneToCheck);
            if (closestVehicleAhead is null && closestVehicleBehind is null)
            {
                return MergeToLane(vehicle, laneToCheck);
            }

            var hasVehicleAhead = closestVehicleAhead != null;
            var hasVehicleBehind = closestVehicleBehind != null;

            if (hasVehicleAhead && closestVehicleAhead.Position.Back < vehicle.Position.Front + vehicle.ComfortableMargin)
            {
                return AdjustSpeed(vehicle, vehicleAhead.Speed);
            }

            if (hasVehicleAhead && closestVehicleAhead.Position.Back < vehicle.Position.Front + vehicle.ComfortableMargin * 2 && closestVehicleAhead.Speed < vehicleAhead.Speed)
            {
                return AdjustSpeed(vehicle, vehicleAhead.Speed);
            }

            if (hasVehicleBehind && closestVehicleBehind.Position.Front > vehicle.Position.Back - vehicle.ComfortableMargin)
            {
                return AdjustSpeed(vehicle, vehicleAhead.Speed);
            }

            if (hasVehicleBehind && closestVehicleBehind.Position.Front > vehicle.Position.Back - vehicle.ComfortableMargin * 2 && closestVehicleBehind.Speed > vehicle.Speed)
            {
                return AdjustSpeed(vehicle, vehicleAhead.Speed);
            }

            return MergeToLane(vehicle, laneToCheck);
        }

        private Vehicle AdjustSpeed(Vehicle vehicle, int speed)
        {
            vehicle.Speed = speed;
            vehicle.MoveForward(speed);
            vehicleService.Update(vehicle);
            return vehicle;
        }

        private Vehicle? GetClosestVehicleAhead(Vehicle vehicle, IEnumerable<Vehicle> nearbyVehicles, int laneToCheck)
        {
            return nearbyVehicles.Where(x => x.Position.LaneNumber == laneToCheck && x.Position.Front >= vehicle.Position.Front).OrderBy(x => x.Position.Back).FirstOrDefault();
        }

        private Vehicle? GetClosestVehicleBehind(Vehicle vehicle, IEnumerable<Vehicle> nearbyVehicles, int laneToCheck)
        {
            return nearbyVehicles.Where(x => x.Position.LaneNumber == laneToCheck && x.Position.Front <= vehicle.Position.Front).OrderBy(x => x.Position.Front).FirstOrDefault();
        }

        private Vehicle MergeToLane(Vehicle vehicle, int lane)
        {
            vehicle.Position.LaneNumber = lane;
            var speed = vehicle.Driver.DesiredSpeed;
            vehicle.Speed = speed;
            vehicle.MoveForward(speed);
            vehicleService.Update(vehicle);
            return vehicle;
        }

        private bool CanMoveLeft(Vehicle vehicle, Road road)
        {
            return vehicle.Position.LaneNumber < road.Lanes - 1;
        }

        private bool CanMoveRight(Vehicle vehicle)
        {
            return vehicle.Position.LaneNumber > 0;
        }

        private Vehicle RestoreSpeedAndMoveForward(Vehicle vehicle)
        {
            var speed = vehicle.Driver.DesiredSpeed;
            vehicle.Speed = speed;
            vehicle.MoveForward(speed);
            vehicleService.Update(vehicle);
            return vehicle;
        }
    }
}
