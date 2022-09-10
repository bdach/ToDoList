namespace Ticketron.DB.Models;

/// <summary>
/// Represents a group of tasks to be done.
/// </summary>
public class TaskGroup
{
    public int Id { get; internal set; }

    /// <summary>
    /// The string to be used as the icon that represents the task group.
    /// Intended usage is to select an emoji here.
    /// </summary>
    public string Icon { get; set; } = "✔";

    /// <summary>
    /// The name of the group.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
