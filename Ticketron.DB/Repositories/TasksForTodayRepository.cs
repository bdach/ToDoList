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

        var tasksWorkedOnToday = (await connection.QueryAsync<Models.Task>(
            @"SELECT *
            FROM Tasks tasks
            WHERE
                EXISTS
	            (
		            SELECT 1
		            FROM TaskLogEntries logEntries
		            WHERE logEntries.TaskId = tasks.Id
		            AND DATE(logEntries.Start) = DATE(@Today)
	            )
	            AND Done = 0",
            new
            {
                Today = today
            }))
            .ToList();

        var overdueTasks = (await connection.QueryAsync<Models.Task>(
            @"SELECT * FROM Tasks
            WHERE ScheduledFor < @Today
	            AND Done = 0
	            AND NOT EXISTS
	            (
		            SELECT 1
		            FROM TaskLogEntries logEntries
		            WHERE logEntries.TaskId = tasks.Id
		            AND DATE(logEntries.Start) = DATE(@Today)
	            )",
            new
            {
                Today = today
            }))
            .ToList();

        var tasksScheduledForToday = (await connection.QueryAsync<Models.Task>(
            @"SELECT * FROM Tasks
            WHERE DATE(ScheduledFor) = DATE(@Today)
                AND Done = 0
	            AND NOT EXISTS
	            (
		            SELECT 1
		            FROM TaskLogEntries logEntries
		            WHERE logEntries.TaskId = tasks.Id
		            AND DATE(logEntries.Start) = DATE(@Today)
	            )",
            new
            {
                Today = today
            }))
            .ToList();

        var tasksDoneToday = (await connection.QueryAsync<Models.Task>(
            @"SELECT * FROM Tasks
                WHERE
	                (
		                DATE(ScheduledFor) = DATE(@Today)
		                OR
                        EXISTS
		                (
			                SELECT 1
			                FROM TaskLogEntries logEntries
			                WHERE logEntries.TaskId = tasks.Id
			                AND DATE(logEntries.Start) = DATE(@Today)
		                )
	                )
	                AND Done = 1",
            new
            {
                Today = today
            }))
            .ToList();

        return new Models.TasksForToday
        {
            TasksWorkedOnToday = tasksWorkedOnToday,
            OverdueTasks = overdueTasks,
            TasksScheduledForToday = tasksScheduledForToday,
            TasksDoneToday = tasksDoneToday
        };
    }
}