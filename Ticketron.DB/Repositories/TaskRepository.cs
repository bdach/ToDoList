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

        await connection.ExecuteAsync(
            @"INSERT INTO Tasks
                    (Id, Title, Done)
                VALUES
                    (@id, @title, @done)", task);
    }

    public async Task UpdateAsync(Models.Task task)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(
            @"UPDATE Tasks
            SET Title = @title,
                Done = @done
            WHERE Id = @id", task);
    }

    public async Task DeleteAsync(Models.Task task)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(@"DELETE FROM Tasks WHERE Id = @id", task);
    }
}