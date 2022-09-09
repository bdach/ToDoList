namespace Ticketron.DB.Repositories;

public interface ITaskGroupRepository
{
    /// <summary>
    /// Returns all <see cref="Models.TaskGroup"/>s in the database.
    /// </summary>
    Task<ICollection<Models.TaskGroup>> GetAllAsync();

    /// <summary>
    /// Creates the supplied <paramref name="taskGroup"/> in the database.
    /// </summary>
    Task CreateAsync(Models.TaskGroup taskGroup);

    /// <summary>
    /// Updates the supplied <paramref name="taskGroup"/> in the database.
    /// </summary>
    Task UpdateAsync(Models.TaskGroup taskGroup);

    /// <summary>
    /// Deletes the supplied <paramref name="taskGroup"/> from the database.
    /// </summary>
    Task DeleteAsync(Models.TaskGroup taskGroup);
}