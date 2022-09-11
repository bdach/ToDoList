namespace Ticketron.DB.Repositories;

public interface ITaskLogRepository
{
    Task<ICollection<Models.TaskLogEntry>> GetAllForTaskAsync(Models.Task task);

    Task CreateAsync(Models.TaskLogEntry logEntry);
    Task UpdateAsync(Models.TaskLogEntry logEntry);
    Task DeleteAsync(Models.TaskLogEntry logEntry);
}