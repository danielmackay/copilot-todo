using Api.Common;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Todos;

public static class GetAllTodos
{
    public static RouteGroupBuilder MapGetAllTodos(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (TodoDbContext db) =>
        {
            var todos = await db.Todos.ToListAsync();
            return Results.Ok(todos);
        })
        .WithName("GetAllTodos")
        .WithDescription("Retrieves all todo items")
        .WithOpenApi();

        return group;
    }
}