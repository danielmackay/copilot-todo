using Api.Common;
using Microsoft.OpenApi.Models;

namespace Api.Features.Todos;

public static class DeleteTodo
{
    public static RouteGroupBuilder MapDeleteTodo(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (Guid id, TodoDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();

            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteTodo")
        .WithDescription("Delete a todo")
        .WithOpenApi();

        return group;
    }
}