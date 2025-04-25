using Api.Common;
using Api.Common.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
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

// Seed some initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    // Add some sample todos
    dbContext.Todos.Add(Todo.Create("Learn .NET 9"));
    dbContext.Todos.Add(Todo.Create("Build a Todo App"));
    dbContext.Todos.Add(Todo.Create("Learn React with TypeScript"));
    dbContext.SaveChanges();
}

// Todo Endpoints
// GET /todos - Get all todos
app.MapGet("/todos", async (TodoDbContext db) =>
{
    var todos = await db.Todos.ToListAsync();
    return Results.Ok(todos);
})
.WithName("GetAllTodos");

// GET /todos/{id} - Get a specific todo
app.MapGet("/todos/{id}", async (Guid id, TodoDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
.WithName("GetTodoById");

// POST /todos - Create a new todo
app.MapPost("/todos", async (TodoCreateRequest request, TodoDbContext db) =>
{
    try
    {
        var todo = Todo.Create(request.Title);
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return Results.Created($"/todos/{todo.Id}", todo);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("CreateTodo");

// PUT /todos/{id} - Update a todo
app.MapPut("/todos/{id}", async (Guid id, TodoUpdateRequest request, TodoDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    try
    {
        if (request.Title is not null)
        {
            todo.UpdateTitle(request.Title);
        }

        if (request.IsCompleted.HasValue)
        {
            if (request.IsCompleted.Value)
            {
                todo.MarkComplete();
            }
            else
            {
                todo.MarkIncomplete();
            }
        }

        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("UpdateTodo");

// DELETE /todos/{id} - Delete a todo
app.MapDelete("/todos/{id}", async (Guid id, TodoDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteTodo");

app.Run();

// Add the request record types
record TodoCreateRequest(string Title);
record TodoUpdateRequest(string? Title, bool? IsCompleted);

