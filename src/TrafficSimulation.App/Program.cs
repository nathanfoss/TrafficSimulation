using MathNet.Numerics.Distributions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrafficSimulation.Application.Roads;
using TrafficSimulation.Application.Vehicles;
using TrafficSimulation.Domain.Roads;
using TrafficSimulation.Domain.Vehicles;
using TrafficSimulation.Infrastructure.Roads;
using TrafficSimulation.Infrastructure.Vehicles;

var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

var services = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IRoadService, RoadService>()
    .AddSingleton<IVehicleService, VehicleService>()
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(AddRoadCommand).Assembly);
    });
var serviceProvider = services.BuildServiceProvider();
var mediatr = serviceProvider.GetRequiredService<IMediator>();

Console.WriteLine("Please enter a speed limit (10-80 mph)");
var speed = int.Parse(Console.ReadLine());

Console.WriteLine("Please enter the number of lanes (1-7)");
var lanes = int.Parse(Console.ReadLine());

await mediatr.Send(new AddRoadCommand
{
    Road = new Road
    {
        SpeedLimit = speed,
        Lanes = lanes
    }
});

var random = new Random();
var normal = new Normal(speed, 10);

var vehicleTypes = new List<VehicleType>
{
    VehicleTypes.Compact,
    VehicleTypes.Sedan,
    VehicleTypes.Pickup,
    VehicleTypes.Minivan,
    VehicleTypes.Semi,
    VehicleTypes.Motorcycle
};
var vehicles = Enumerable.Range(0, 10).Select(_ =>
{
    var position = random.Next(-5280, 5280);
    var vehicleType = vehicleTypes[random.Next(0, vehicleTypes.Count)];
    return new Vehicle
    {
        Id = Guid.NewGuid(),
        Position = new VehiclePosition
        {
            LaneNumber = random.Next(0, lanes),
            Front = position,
            Back = position - vehicleType.Size
        },
        VehicleType = vehicleType,
        Speed = Convert.ToInt32(normal.Sample())
    };
}).OrderByDescending(x => x.Position.Front);

await mediatr.Send(new AddVehiclesCommand { Vehicles = vehicles });
foreach (var vehicle in vehicles)
{
    Console.WriteLine($"{vehicle}");
}
