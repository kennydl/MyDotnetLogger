using System;

namespace My.Dotnet.Logger.TableStorage.Utilities
{
    public static class AzureStorageUtil
    {
        public static Microsoft.Azure.Cosmos.Table.CloudStorageAccount GetStorageAccount(string storageConnectionString)
        {
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount storageAccount;
            try
            {
                storageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                throw;
            }

            return storageAccount;
        }

        public static Microsoft.WindowsAzure.Storage.CloudStorageAccount GetLoggerStorageAccount(string storageConnectionString)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount;
            try
            {
                storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                throw;
            }

            return storageAccount;
        }
    }
}
