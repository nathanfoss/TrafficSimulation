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
var vehicles = Enumerable.Range(0, 100).Select(_ => new Vehicle
{
    Id = Guid.NewGuid(),
    LaneNumber = random.Next(0, lanes),
    Position = random.Next(-1000, 1000),
    Speed = random.Next(0, speed + 20)
}).OrderBy(x => x.Position);

await mediatr.Send(new AddVehiclesCommand { Vehicles = vehicles });
foreach (var vehicle in vehicles)
{
    Console.WriteLine($"{vehicle}");
}
