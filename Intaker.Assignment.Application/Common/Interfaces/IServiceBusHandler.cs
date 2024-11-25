namespace Intaker.Assignment.Application.Common.Interfaces;
public interface IServiceBusHandler
{
    Task Initialize(CancellationToken token = default);

    Task SendMessage(Domain.Task task, CancellationToken token = default);

    Task ReceiveMessage(CancellationToken token = default);
}
