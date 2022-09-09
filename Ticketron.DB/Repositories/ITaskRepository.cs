namespace Ticketron.DB.Repositories;

public interface ITaskRepository
{
    /// <summary>
    /// Returns all <see cref="Models.Task"/>s in the database.
    /// </summary>
    Task<ICollection<Models.Task>> GetAllAsync();

    /// <summary>
    /// Returns all <see cref="Models.Task"/>s from the supplied <paramref name="group"/>.
    /// </summary>
    Task<ICollection<Models.Task>> GetForGroupAsync(Models.TaskGroup group);

    /// <summary>
    /// Creates the supplied <paramref name="task"/> in the database.
    /// </summary>
    Task CreateAsync(Models.Task task);

    /// <summary>
    /// Updates the supplied <paramref name="task"/> in the database.
    /// </summary>
    Task UpdateAsync(Models.Task task);

    /// <summary>
    /// Deletes the supplied <paramref name="task"/> from    the database.
    /// </summary>
    Task DeleteAsync(Models.Task task);
}