using Dapper;

namespace Ticketron.DB.Repositories;

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
                WHERE TaskId = @Id",
            task)).ToList();
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