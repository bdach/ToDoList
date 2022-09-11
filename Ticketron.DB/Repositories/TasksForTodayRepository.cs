using Dapper;

namespace Ticketron.DB.Repositories;

public class TasksForTodayRepository : ITasksForTodayRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TasksForTodayRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Models.TasksForToday> GetTasksForTodayAsync(DateTime today)
    {
        using var connection = _connectionFactory.Create();

        var overdueTasks = (await connection.QueryAsync<Models.Task>(
            @"SELECT * FROM Tasks
            WHERE ScheduledFor < @Today
                AND Done = 0",
            new
            {
                Today = today
            }))
            .ToList();

        var tasksScheduledForToday = (await connection.QueryAsync<Models.Task>(
            @"SELECT * FROM Tasks
            WHERE DATE(ScheduledFor) = DATE(@Today)
                AND Done = 0",
            new
            {
                Today = today
            }))
            .ToList();

        // TODO: When time tracking lands, also return tasks that weren't necessarily scheduled for today, but _have_ been worked on today.
        var tasksDoneToday = (await connection.QueryAsync<Models.Task>(
            @"SELECT * FROM Tasks
            WHERE DATE(ScheduledFor) = DATE(@Today)
                AND Done = 1",
            new
            {
                Today = today
            }))
            .ToList();

        return new Models.TasksForToday
        {
            OverdueTasks = overdueTasks,
            TasksScheduledForToday = tasksScheduledForToday,
            TasksDoneToday = tasksDoneToday
        };
    }
}