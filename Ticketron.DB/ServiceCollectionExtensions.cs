using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Ticketron.DB.Repositories;

namespace Ticketron.DB;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all components required to query the Ticketron database from the given <paramref name="path"/> to the supplied <paramref name="serviceCollection"/>.
    /// </summary>
    public static IServiceCollection AddTicketronDatabase(this IServiceCollection serviceCollection, string path)
    {
        var connectionString = $"Data Source={path}";

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
