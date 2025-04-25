using Api.Common;
using Api.Common.Domain;
using Microsoft.OpenApi.Models;

namespace Api.Features.Todos;

public static class CreateTodo
{
    public record TodoCreateRequest(string Title);

    public static RouteGroupBuilder MapCreateTodo(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (TodoCreateRequest request, TodoDbContext db) =>
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
        .WithName("CreateTodo")
        .WithDescription("Create a new todo")
        .WithOpenApi();

        return group;
    }
}