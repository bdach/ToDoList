namespace Ticketron.DB.Models;

/// <summary>
/// Represents a single task to be done.
/// </summary>
public class Task
{
    public long Id { get; init; }

    /// <summary>
    /// The title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Whether the task is done.
    /// </summary>
    public bool Done { get; set; }
}