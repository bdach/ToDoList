namespace ToDoList.DB.Models;

public class TaskLogEntry
{
    public long Id { get; internal set; }

    public long TaskId { get; internal set; }

    public DateTime Start { get; set; }
    public DateTime? End { get; set; }

    public TimeSpan TimeLogged => (End ?? DateTime.Now) - Start;

    public string Notes { get; set; } = string.Empty;

    public TaskLogEntry(Task task)
    {
        TaskId = task.Id;
        Start = DateTime.Now;
    }
}