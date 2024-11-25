namespace Intaker.Assignment.Application.Common.Interfaces;
public interface IEventProcessor
{
    Task ProcessEvent(string message);
}
