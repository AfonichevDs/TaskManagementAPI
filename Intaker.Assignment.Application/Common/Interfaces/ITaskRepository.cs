using Intaker.Assignment.Application.Domain.Enums;

namespace Intaker.Assignment.Application.Common.Interfaces;
public interface ITaskRepository
{
    Task<Domain.Task?> Get(int id, CancellationToken token = default);

    Task<List<Domain.Task>> GetAll(CancellationToken token = default);

    Task<Domain.Task> Add(Domain.Task task, CancellationToken token = default);
    Task<int> SaveChanges(CancellationToken token = default);
}
