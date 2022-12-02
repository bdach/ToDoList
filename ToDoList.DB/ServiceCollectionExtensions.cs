using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.DB.Repositories;

namespace ToDoList.DB;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all components required to query the ToDoList database from the given <paramref name="path"/> to the supplied <paramref name="serviceCollection"/>.
    /// </summary>
    public static IServiceCollection AddToDoListDatabase(this IServiceCollection serviceCollection, string path)
    {
        var connectionString = $"Data Source={path};Foreign Keys = True";

        serviceCollection
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(ServiceCollectionExtensions).Assembly).For.Migrations())
            .AddLogging(logging => logging.AddFluentMigratorConsole());

        serviceCollection.AddSingleton<IDbConnectionFactory>(_ => new SqliteDbConnectionFactory(connectionString));

        serviceCollection.AddSingleton<ITaskRepository, TaskRepository>();
        serviceCollection.AddSingleton<ITaskGroupRepository, TaskGroupRepository>();
        serviceCollection.AddSingleton<ITasksForTodayRepository, TasksForTodayRepository>();
        serviceCollection.AddSingleton<ITaskLogRepository, TaskLogRepository>();

        return serviceCollection;
    }
}
