using MediatR;
using Microsoft.Extensions.Logging;
using TrafficSimulation.Domain.Roads;

namespace TrafficSimulation.Application.Roads
{
    public class AddRoadCommand : IRequest<Result<Road>>
    {
        public Road Road { get; set; }
    }

    public class AddRoadCommandHandler : IRequestHandler<AddRoadCommand, Result<Road>>
    {
        private readonly IRoadService roadService;

        private readonly ILogger<AddRoadCommandHandler> logger;

        public AddRoadCommandHandler(IRoadService roadSerivce, ILogger<AddRoadCommandHandler> logger)
        {
            this.roadService = roadSerivce;
            this.logger = logger;
        }

        public Task<Result<Road>> Handle(AddRoadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                roadService.Road = request.Road;
                return Task.FromResult(Result<Road>.Success(request.Road));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding road {@road}", request.Road);
                return Task.FromResult(Result<Road>.Failure(ex));
            }
        }
    }
}
