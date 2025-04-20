using System.Data.Common;
using InventoryHubAPI.Shared;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace InventoryHubAPI.Data
{
    internal class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;
        private readonly string _providerName;

        public DatabaseConnection(IConfiguration configuration, IOptions<AppSettings> options)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Database connection string is missing.");

            _providerName = options.Value.DatabaseProvider//configuration["DatabaseProvider"]  //options.Value.DatabaseProvider
                ?? throw new ArgumentNullException(nameof(configuration), "Database provider name is missing.");
        }

        public async Task<DbConnection> CreateConnectionAsync()
        {
            DbProviderFactory? factory = DbProviderFactories.GetFactory(_providerName) 
                ?? throw new InvalidOperationException($"Unsupported database provider: {_providerName}");

            DbConnection connection = factory.CreateConnection() 
                ?? throw new InvalidOperationException("Failed to create a database connection.");

            connection.ConnectionString = _connectionString;
            await connection.OpenAsync();
            return connection;
        }

        public string TryGetDbProviderFactory(out DbProviderFactory? factory)
        {
            factory = DbProviderFactories.GetFactory(_providerName);

            if (factory == null)
            {
                return "Database provider factory is missing."; // Return error message instead of throwing
            }

            return string.Empty;
        }
    }
}