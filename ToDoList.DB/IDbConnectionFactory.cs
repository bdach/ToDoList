using System.Data;

namespace ToDoList.DB;

/// <summary>
/// Automatically handles setting up DB connections to query from.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Returns a <see cref="IDbConnection"/> to query from.
    /// Retrieved connection must be disposed by the user.
    /// </summary>
    IDbConnection Create();
}