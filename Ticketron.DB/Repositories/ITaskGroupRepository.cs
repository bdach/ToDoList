namespace Ticketron.DB.Repositories;

public interface ITaskGroupRepository
{
    /// <summary>
    /// Returns all <see cref="Models.TaskGroup"/>s in the database.
    /// </summary>
    Task<ICollection<Models.TaskGroup>> GetAllAsync();

    /// <summary>
    /// Creates the supplied <paramref name="taskGroups"/> in the database.
    /// </summary>
    Task CreateAsync(params Models.TaskGroup[] taskGroups);

    /// <summary>
    /// Updates the supplied <paramref name="taskGroups"/> in the database.
    /// </summary>
    Task UpdateAsync(params Models.TaskGroup[] taskGroups);

    /// <summary>
    /// Deletes the supplied <paramref name="taskGroups"/> from the database.
    /// </summary>
    Task DeleteAsync(params Models.TaskGroup[] taskGroups);
}