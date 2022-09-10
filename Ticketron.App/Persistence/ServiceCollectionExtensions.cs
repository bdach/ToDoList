using Microsoft.Extensions.DependencyInjection;

namespace Ticketron.App.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceManagers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<AppStatePersistenceManager>();
        serviceCollection.AddSingleton<TaskGroupPagePersistenceManager>();

        return serviceCollection;
    }
}