namespace ToDoList.DB.Models
{
    public class DailyLog
    {
        public ICollection<DailyTaskProgress> TaskProgress { get; internal set; } = new List<DailyTaskProgress>();
    }

    public class DailyTaskProgress
    {
        public TaskGroup Group { get; }
        public Task Task { get; }
        public ICollection<TaskLogEntry> Entries { get; internal set; } = new List<TaskLogEntry>();

        public TimeSpan TotalTimeLogged =>
            Entries.Aggregate(TimeSpan.Zero, (total, entry) => total += entry.TimeLogged);

        public DailyTaskProgress(TaskGroup group, Task task)
        {
            Group = group;
            Task = task;
        }
    }

    internal class FlattenedLogEntry
    {
        public long LogEntryId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Notes { get; set; } = string.Empty;
        public long TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public DateTime? TaskScheduledFor { get; set; }
        public bool TaskDone { get; set; }
        public int TaskGroupId { get; set; }
        public string TaskGroupIcon { get; set; } = string.Empty;
        public string TaskGroupName { get; set; } = string.Empty;
    }
}
