using Intaker.Assignment.Application.Common.Interfaces;
using Intaker.Assignment.Application.Domain.Enums;
using Intaker.Assignment.Application.Integration;
using Intaker.Assignment.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intaker.Assignment.Features.Tasks.Commands;

public static class UpdateStatus
{
    public record Command(int Id, Status Status) : IRequest<Response<Application.Domain.Task>>;

    public class Handler(ITaskRepository repository, IServiceBusHandler serviceBusHandler) : IRequestHandler<Command, Response<Application.Domain.Task>>
    {
        public async Task<Response<Application.Domain.Task>> Handle(Command request, CancellationToken cancellationToken)
        {
            var task = await repository.Get(request.Id, cancellationToken);

            if (task == null)
                throw new ArgumentException(nameof(request.Id)); // for best practice can define custom exception and use middleware to return corresponding status

            task.Status = request.Status;
            await repository.SaveChanges(cancellationToken);

            await serviceBusHandler.SendMessage(task, cancellationToken);

            return new(task);
        }
    }

    public static void MapEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPatch("api/tasks/status", async ([FromBody]Command query, ISender sender) =>
        {
            return Results.Ok(await sender.Send(query));
        });
    }
}
