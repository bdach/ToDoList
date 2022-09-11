namespace Ticketron.DB.Repositories;

public interface ITasksForTodayRepository
{
    Task<Models.TasksForToday> GetTasksForTodayAsync(DateTime today);
}