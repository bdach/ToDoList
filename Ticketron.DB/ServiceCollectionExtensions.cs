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

        serviceCollection.AddTransient<IDbConnectionFactory>(_ => new SqliteDbConnectionFactory(connectionString));

        serviceCollection.AddTransient<ITaskRepository, TaskRepository>();
        serviceCollection.AddTransient<ITaskGroupRepository, TaskGroupRepository>();

        return serviceCollection;
    }
}
