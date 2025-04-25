namespace Api.Common.Domain;

public class Todo
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Todo() { }

    public static Todo Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        return new Todo
        {
            Id = Guid.NewGuid(),
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkComplete()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public void MarkIncomplete()
    {
        if (IsCompleted)
        {
            IsCompleted = false;
            CompletedAt = null;
        }
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
    }
}