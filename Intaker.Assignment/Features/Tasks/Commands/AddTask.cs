using Intaker.Assignment.Application.Common.Interfaces;
using Intaker.Assignment.Application.Domain.Enums;
using Intaker.Assignment.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intaker.Assignment.Features.Tasks.Commands;

public static class AddTask
{
    public record Command(string Name, string Description, Status Status, string? AssignedTo) : IRequest<Response<Application.Domain.Task>>;

    public class Handler(ITaskRepository repository, IServiceBusHandler serviceBusHandler) : IRequestHandler<Command, Response<Application.Domain.Task>>
    {
        public async Task<Response<Application.Domain.Task>> Handle(Command request, CancellationToken cancellationToken)
        {
            var task = new Application.Domain.Task 
            { 
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                AssignedTo = request.AssignedTo,
            };

            await repository.Add(task, cancellationToken);
            await repository.SaveChanges(cancellationToken);

            await serviceBusHandler.SendMessage(task, cancellationToken);

            return new(task);
        }
    }

    public static void MapEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("api/tasks", async ([FromBody]Command query, ISender sender) =>
        {
            return Results.Ok(await sender.Send(query));
        });
    }
}
