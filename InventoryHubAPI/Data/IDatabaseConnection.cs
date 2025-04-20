using System.Data.Common;

namespace InventoryHubAPI.Data
{
    public interface IDatabaseConnection
    {
        // support multiple databases
        Task<DbConnection> CreateConnectionAsync();

        string TryGetDbProviderFactory(out DbProviderFactory? factory);
    }
}