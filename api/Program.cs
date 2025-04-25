using Api.Common;
using Api.Common.Domain;
using Api.Features.Todos;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseInMemoryDatabase("TodoList"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure the port to 5000
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors("AllowAll");

// Enable Scalar UI for OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Serves the OpenAPI document
    app.MapScalarApiReference(options => options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient));

}

// Seed some initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    // Add some sample todos if none exist
    if (!dbContext.Todos.Any())
    {
        dbContext.Todos.Add(Todo.Create("Learn .NET 9"));
        dbContext.Todos.Add(Todo.Create("Build a Todo App"));
        dbContext.Todos.Add(Todo.Create("Learn React with TypeScript"));
        dbContext.SaveChanges();
    }
}

// Map all Todo endpoints using Vertical Slice Architecture
var todoGroup = app.MapGroup("/todos").WithOpenApi();
todoGroup.MapGetAllTodos();
todoGroup.MapGetTodoById();
todoGroup.MapCreateTodo();
todoGroup.MapUpdateTodo();
todoGroup.MapDeleteTodo();

app.Run();
