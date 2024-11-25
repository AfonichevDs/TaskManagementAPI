using Intaker.Assignment.Application.Common.Interfaces;
using Intaker.Assignment.Application.Infrastracture.Persistence;
using Intaker.Assignment.Application.Infrastracture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Intaker.Tests;

public class Tests
{
    private DbContextOptions<ApplicationDbContext> _options;
    private ApplicationDbContext _context;
    private ITaskRepository _repository;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;

        _context = new ApplicationDbContext(_options);
        _repository = new TaskRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void Add_Task_Should_Add_Task_To_Database()
    {
        var task = new Assignment.Application.Domain.Task { ID = 10, Name = "Test Task", Description ="Desc", Status = Assignment.Application.Domain.Enums.Status.NotStarted };

        _repository.Add(task);
        _context.SaveChanges();

        var tasksInDb = _context.Tasks.ToList();
        Assert.That(tasksInDb.Count, Is.EqualTo(1));
        Assert.That(tasksInDb[0].Name, Is.EqualTo("Test Task"));
    }
}