using Intaker.Assignment.Application.Common.Interfaces;
using Intaker.Assignment.Application.Infrastracture.Persistence;
using Intaker.Assignment.Application.Infrastracture.Repositories;
using Intaker.Assignment.Application.Integration;
using Intaker.Assignment.Features.Tasks.Commands;
using Intaker.Assignment.Features.Tasks.Queries;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.CustomSchemaIds(type =>
{
    if (type.DeclaringType != null)
    {
        return type.DeclaringType.Name + type.Name;
    }
    return type.Name;
}));
builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddTransient<ITaskRepository, TaskRepository>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddSingleton<IServiceBusHandler, ServiceBusHandler>();
builder.Services.AddHostedService<ServiceBusConsumer>();


builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("tasksDb"));


var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

GetAllTasks.MapEndpoint(app);
AddTask.MapEndpoint(app);
UpdateStatus.MapEndpoint(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
