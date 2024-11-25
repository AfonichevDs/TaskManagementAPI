using Intaker.Assignment.Application.Common.Interfaces;

namespace Intaker.Assignment.Application.Integration;
public class EventProcessor : IEventProcessor
{
    public async Task ProcessEvent(string message)
    {
        await Task.Delay(100); //process event
        
        Console.WriteLine("Action successful, following data received: ");
        Console.WriteLine(message);
    }
}
