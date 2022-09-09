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

    public async Task CreateAsync(Models.TaskGroup taskGroup)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(
            @"INSERT INTO TaskGroups
                    (Id, Icon, Name)
                VALUES
                    (@id, @icon, @name)", taskGroup);
    }

    public async Task UpdateAsync(Models.TaskGroup taskGroup)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(
            @"UPDATE TaskGroups
                SET Icon = @icon,
                    Name = @name
                WHERE Id = @id", taskGroup);
    }

    public async Task DeleteAsync(Models.TaskGroup taskGroup)
    {
        using var connection = _dbConnectionFactory.Create();

        await connection.ExecuteAsync(@"DELETE FROM TaskGroups WHERE Id = @id", taskGroup);
    }
}