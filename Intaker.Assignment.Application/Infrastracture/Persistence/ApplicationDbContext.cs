using Intaker.Assignment.Application.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Task = Intaker.Assignment.Application.Domain.Task;

namespace Intaker.Assignment.Application.Infrastracture.Persistence;
public class ApplicationDbContext : DbContext
{
    public DbSet<Task> Tasks { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt): base(opt)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>().HasData(
            new Task
            {
                ID = 1,
                Name = "Service Bus",
                Description = "Implement a ServiceBusHandler class to manage sending and receiving messages.",
                Status = Status.InProgress,
                AssignedTo = "Denys"
            },
            new Task
            {
                ID = 2,
                Name = "Develop a frontend",
                Description = "Please develop a frontend that utilizes the API methods.",
                Status = Status.NotStarted,
                AssignedTo = "FullstackDev"
            },
            new Task
            {
                ID = 4,
                Name = "Pet a dog",
                Description = "Just pet any dog. ID 3 is missing.",
                Status = Status.Completed
            }
        );
    }
}
