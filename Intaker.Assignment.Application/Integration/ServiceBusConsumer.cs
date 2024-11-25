using Intaker.Assignment.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Intaker.Assignment.Application.Integration;
public class ServiceBusConsumer : BackgroundService
{
    private readonly IServiceBusHandler _serviceBusHandler;

    public ServiceBusConsumer(IServiceBusHandler serviceBusHandler)
    {
        _serviceBusHandler = serviceBusHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _serviceBusHandler.Initialize(stoppingToken);
        await _serviceBusHandler.ReceiveMessage(stoppingToken);
    }
}
