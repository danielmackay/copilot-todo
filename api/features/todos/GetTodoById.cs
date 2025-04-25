using Api.Common;
using Microsoft.OpenApi.Models;

namespace Api.Features.Todos;

public static class GetTodoById
{
    public static RouteGroupBuilder MapGetTodoById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (Guid id, TodoDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        })
        .WithName("GetTodoById")
        .WithOpenApi();

        return group;
    }
}