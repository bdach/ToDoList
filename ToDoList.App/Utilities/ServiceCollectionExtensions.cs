using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ToDoList.App.Utilities;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogLogging(this IServiceCollection serviceCollection, string logFilePath)
    {
        serviceCollection.AddSingleton<ILogger>(_ => new LoggerConfiguration()
            .WriteTo.File(logFilePath)
            .CreateLogger());

        return serviceCollection;
    }
}