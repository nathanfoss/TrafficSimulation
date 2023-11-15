using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrafficSimulation.Application.Roads;
using TrafficSimulation.Domain.Roads;
using TrafficSimulation.Infrastructure.Roads;

var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

var services = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IRoadService, RoadService>()
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
