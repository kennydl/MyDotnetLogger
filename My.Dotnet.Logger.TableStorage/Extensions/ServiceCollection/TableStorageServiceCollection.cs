using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using My.Dotnet.Logger.Data.Context;
using My.Dotnet.Logger.TableStorage.Factories;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using My.Dotnet.Logger.TableStorage.Repositories;
using My.Dotnet.Logger.TableStorage.Utilities;

namespace My.Dotnet.Logger.TableStorage.Extensions.ServiceCollection
{
    public static class TableStorageServiceCollection
    {
        public static CloudStorageAccount AddStorageAccount(this IServiceCollection services, string connectionString)
        {           
            var storageAccount = AzureStorageUtil.GetStorageAccount(connectionString);
            services.AddSingleton(storageAccount);
            return storageAccount;
        }

        public static IServiceCollection AddTableStorageLogRepository(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogServiceContext, LogServiceContext>();
            services.AddSingleton<ILogRepositoryFactory, LogRepositoryFactory>();
            services.AddScoped(provider => provider.GetService<ILogRepositoryFactory>().CreateRepository());
            return services;
        }
    }
}
