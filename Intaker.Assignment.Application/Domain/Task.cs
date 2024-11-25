using Intaker.Assignment.Application.Domain.Enums;

namespace Intaker.Assignment.Application.Domain;
public class Task
{
    public int ID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Status Status { get; set; }

    public string? AssignedTo { get; set; }
}
