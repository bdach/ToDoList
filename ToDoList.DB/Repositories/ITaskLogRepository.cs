using ToDoList.DB.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.DB.Repositories;

public interface ITaskLogRepository
{
    Task<ICollection<Models.TaskLogEntry>> GetAllForTaskAsync(Models.Task task);
    Task<DailyLog> GetDailyLogAsync(DateTime date);

    Task CreateAsync(Models.TaskLogEntry logEntry);
    Task UpdateAsync(Models.TaskLogEntry logEntry);
    Task DeleteAsync(Models.TaskLogEntry logEntry);
}