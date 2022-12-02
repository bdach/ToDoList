using Dapper;
using ToDoList.DB.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.DB.Repositories;

public class TaskLogRepository : ITaskLogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TaskLogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ICollection<Models.TaskLogEntry>> GetAllForTaskAsync(Models.Task task)
    {
        using var connection = _connectionFactory.Create();

        return (await connection.QueryAsync<Models.TaskLogEntry>(
            @"SELECT *
                FROM TaskLogEntries
                WHERE TaskId = @Id
                ORDER BY Start ASC",
            task)).ToList();
    }

    public async Task<Models.DailyLog> GetDailyLogAsync(DateTime date)
    {
        using var connection = _connectionFactory.Create();

        var logEntries = (await connection.QueryAsync<FlattenedLogEntry>(
            @"SELECT
	                logEntries.Id AS LogEntryId,
	                logEntries.Start,
	                logEntries.End,
	                logEntries.Notes,
	                tasks.Id AS TaskId,
	                tasks.Title AS TaskTitle,
	                tasks.ScheduledFor AS TaskScheduledFor,
	                tasks.Done AS TaskDone,
	                taskGroups.Id AS TaskGroupId,
	                taskGroups.Icon AS TaskGroupIcon,
	                taskGroups.Name AS TaskGroupName
                FROM TaskLogEntries logEntries
                JOIN Tasks tasks
	                ON logEntries.TaskId = tasks.Id
                JOIN TaskGroups taskGroups
	                ON tasks.GroupId = taskGroups.Id
                WHERE DATE(logEntries.Start) = DATE(@Date)
                ORDER BY logEntries.Start ASC",
            new { Date = date })).ToList();

        var result = new Models.DailyLog();

        foreach (var grouping in logEntries.GroupBy(e => e.TaskId))
        {
            var representative = grouping.First();

            var taskGroup = new Models.TaskGroup
            {
                Id = representative.TaskGroupId,
                Icon = representative.TaskGroupIcon,
                Name = representative.TaskGroupName
            };

            var task = new Models.Task
            {
                Id = representative.TaskId,
                GroupId = representative.TaskGroupId,
                Title = representative.TaskTitle,
                ScheduledFor = representative.TaskScheduledFor,
                Done = representative.TaskDone
            };

            var taskProgress = new DailyTaskProgress(taskGroup, task)
            {
                Entries = grouping.Select(entry => new TaskLogEntry(task)
                {
                    Id = entry.LogEntryId,
                    Start = entry.Start,
                    End = entry.End,
                    Notes = entry.Notes
                })
                .ToList()
            };

            result.TaskProgress.Add(taskProgress);
        }

        return result;
    }

    public async Task CreateAsync(Models.TaskLogEntry logEntry)
    {
        using var connection = _connectionFactory.Create();

        logEntry.Id = await connection.QuerySingleAsync<long>(
            @"INSERT INTO TaskLogEntries
                    (TaskId, Start, End, Notes)
                VALUES
                    (@TaskId, @Start, @End, @Notes)
                RETURNING Id",
            logEntry);
    }

    public async Task UpdateAsync(Models.TaskLogEntry logEntry)
    {
        using var connection = _connectionFactory.Create();

        await connection.ExecuteAsync(
            @"UPDATE TaskLogEntries
                SET TaskId = @TaskId,
                    Start = @Start,
                    End = @End,
                    Notes = @Notes
                WHERE Id = @Id",
            logEntry);
    }

    public async Task DeleteAsync(Models.TaskLogEntry logEntry)
    {
        using var connection = _connectionFactory.Create();
        await connection.ExecuteAsync(@"DELETE FROM TaskLogEntries WHERE Id = @Id", logEntry);
    }
}