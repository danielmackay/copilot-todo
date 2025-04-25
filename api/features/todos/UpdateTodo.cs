using Api.Common;

namespace Api.Features.Todos;

public static class UpdateTodo
{
    public record TodoUpdateRequest(string? Title, bool? IsCompleted);

    public static RouteGroupBuilder MapUpdateTodo(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}", async (Guid id, TodoUpdateRequest request, TodoDbContext db) =>
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
        .WithName("UpdateTodo")
        .WithDescription("Update an existing todo")
        .WithOpenApi();

        return group;
    }
}