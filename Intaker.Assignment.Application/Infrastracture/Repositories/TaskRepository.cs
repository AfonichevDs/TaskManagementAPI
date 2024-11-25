using Intaker.Assignment.Application.Common.Interfaces;
using Intaker.Assignment.Application.Domain.Enums;
using Intaker.Assignment.Application.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intaker.Assignment.Application.Infrastracture.Repositories;
public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    public async Task<Domain.Task> Add(Domain.Task task, CancellationToken token = default)
    {
        await context.Tasks.AddAsync(task, token);
        return task;
    }

    public async Task<Domain.Task?> Get(int id, CancellationToken token = default)
    {
        return await context.Tasks.FirstOrDefaultAsync(x =>x.ID == id, token);
    }

    public async Task<List<Domain.Task>> GetAll(CancellationToken token = default)
    {
        return await context.Tasks.AsNoTracking().ToListAsync(token);
    }

    public async Task<Domain.Task> UpdateStatus(int id, Status newStatus, CancellationToken token = default)
    {
        var task = await Get(id, token);
        if(task == null)
        {
            throw new ArgumentException(nameof(id));
        }
        task.Status = newStatus;
        return task;
    }


    public async Task<int> SaveChanges(CancellationToken token = default)
    {
        return await context.SaveChangesAsync();
    }
}
