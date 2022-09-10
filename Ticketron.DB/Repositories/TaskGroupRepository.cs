using Dapper;

namespace Ticketron.DB.Repositories;

public class TaskGroupRepository : ITaskGroupRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TaskGroupRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ICollection<Models.TaskGroup>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.Create();

        return (await connection.QueryAsync<Models.TaskGroup>(@"SELECT * FROM TaskGroups")).ToList();
    }

    public async Task<Models.TaskGroup> GetAsync(int id)
    {
        using var connection = _dbConnectionFactory.Create();

        return await connection.QuerySingleAsync<Models.TaskGroup>(@"SELECT * FROM TaskGroups WHERE Id = @Id",
            new { Id = id });
    }

    public async Task CreateAsync(params Models.TaskGroup[] taskGroups)
    {
        using var connection = _dbConnectionFactory.Create();

        connection.Open();
        using var transaction = connection.BeginTransaction();

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO TaskGroups
                        (Icon, Name)
                    VALUES
                        (@Icon, @Name)
                    RETURNING Id", taskGroup, transaction);
        }

        transaction.Commit();
    }

    public async Task UpdateAsync(params Models.TaskGroup[] taskGroups)
    {
        using var connection = _dbConnectionFactory.Create();

        connection.Open();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            @"UPDATE TaskGroups
                SET Icon = @Icon,
                    Name = @Name
                WHERE Id = @Id", taskGroups, transaction);

        transaction.Commit();
    }

    public async Task DeleteAsync(params Models.TaskGroup[] taskGroups)
    {
        using var connection = _dbConnectionFactory.Create();

        connection.Open();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(@"DELETE FROM TaskGroups WHERE Id = @Id", taskGroups);

        transaction.Commit();
    }
}