namespace Ticketron.DB.Models;

/// <summary>
/// Collections of tasks to show on the "tasks for today" app page.
/// </summary>
public class TasksForToday
{
    /// <summary>
    /// Tasks which were scheduled for a day before today.
    /// </summary>
    public IReadOnlyCollection<Task> OverdueTasks { get; internal set; } = Array.Empty<Task>();

    /// <summary>
    /// Tasks which are scheduled for today.
    /// </summary>
    public IReadOnlyCollection<Task> TasksScheduledForToday { get; internal set; } = Array.Empty<Task>();

    /// <summary>
    /// Tasks which were done today.
    /// TODO: This will be slightly inaccurate until time tracking is in place.
    /// </summary>
    public IReadOnlyCollection<Task> TasksDoneToday { get; internal set; } = Array.Empty<Task>();
}