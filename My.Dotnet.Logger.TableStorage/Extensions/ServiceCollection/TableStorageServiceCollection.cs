using Microsoft.Extensions.DependencyInjection;
using My.Dotnet.Logger.TableStorage.Context;
using My.Dotnet.Logger.TableStorage.Factories;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using My.Dotnet.Logger.TableStorage.Repositories;
using My.Dotnet.Logger.TableStorage.Utilities;

namespace My.Dotnet.Logger.TableStorage.Extensions.ServiceCollection
{
    public static class TableStorageServiceCollection
    {
        public static IServiceCollection AddTableStorageLogRepository(this IServiceCollection services, string connectionString)
        {
            var storageAccount = AzureStorageUtil.GetStorageAccount(connectionString);
            services.AddSingleton(storageAccount)
                .AddScoped<ILogRepository, LogRepository>()
                .AddScoped<ILogServiceContext, LogServiceContext>()
                .AddSingleton<ILogRepositoryFactory, LogRepositoryFactory>()
                .AddScoped(provider => provider.GetService<ILogRepositoryFactory>().CreateRepository());
            return services;
        }
    }
}
