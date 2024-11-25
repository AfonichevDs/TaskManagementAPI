using Intaker.Assignment.Application.Common.Interfaces;
using Intaker.Assignment.Wrappers;
using MediatR;

namespace Intaker.Assignment.Features.Tasks.Queries;

public static class GetAllTasks
{
    public record Query() : IRequest<Response<List<Application.Domain.Task>>>;


    public class Handler(ITaskRepository repository) : IRequestHandler<Query, Response<List<Application.Domain.Task>>>
    {
        public async Task<Response<List<Application.Domain.Task>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tasks = await repository.GetAll(cancellationToken);

            return new(tasks);
        }
    }

    public static void MapEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("api/tasks", async (ISender sender) =>
        {
            return Results.Ok(await sender.Send(new Query()));
        });
    }
}
