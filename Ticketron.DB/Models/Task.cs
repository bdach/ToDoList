namespace Ticketron.DB.Models;

/// <summary>
/// Represents a single task to be done.
/// </summary>
public class Task
{
    public long Id { get; internal set; }

    /// <summary>
    /// The title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Whether the task is done.
    /// </summary>
    public bool Done { get; set; }

    /// <summary>
    /// The ID of the <see cref="TaskGroup"/> that this task belongs to.
    /// </summary>
    public int GroupId { get; internal set; }

    internal Task()
    {
    }

    public Task(TaskGroup group)
    {
        GroupId = group.Id;
    }
}