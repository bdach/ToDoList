using Dapper;

namespace Ticketron.DB.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TaskRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ICollection<Models.Task>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.Create();

        return (await connection.QueryAsync<Models.Task>(@"SELECT * FROM Tasks")).ToList();
    }

    public async Task<ICollection<Models.Task>> GetForGroupAsync(Models.TaskGroup group)
    {
        using var connection = _dbConnectionFactory.Create();

        return (await connection.QueryAsync<Models.Task>(@"SELECT * FROM Tasks WHERE GroupId = @groupId",
            new
            {
                groupId = group.Id
            }))
            .ToList();
    }

    public async Task CreateAsync(Models.Task task)
    {
        using var connection = _dbConnectionFactory.Create();

        task.Id = await connection.QuerySingleAsync<long>(
            @"INSERT INTO Tasks
                    (Title, Done, ScheduledFor, GroupId)
                VALUES
                    (@Title, @Done, @ScheduledFor, @GroupId)
                RETURNING Id", task);
    }

    public async Task UpdateAsync(Models.Task task)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(
            @"UPDATE Tasks
            SET Title = @Title,
                Done = @Done,
                ScheduledFor = @ScheduledFor,
                GroupId = @GroupId
            WHERE Id = @Id", task);
    }

    public async Task DeleteAsync(Models.Task task)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(@"DELETE FROM Tasks WHERE Id = @Id", task);
    }
}