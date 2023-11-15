using Microsoft.Extensions.Logging;
using Moq;

namespace TrafficSimulation.Application.Test
{
    public class RequestTestBase<T>
    {
        public readonly Mock<ILogger<T>> mockLogger = new();

        public T handler;
    }
}
