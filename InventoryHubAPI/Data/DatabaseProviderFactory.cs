using Microsoft.Data.SqlClient;
using System.Data.Common;
using Npgsql;
using MySql.Data.MySqlClient;

namespace InventoryHubAPI.Date
{
    internal static class DatabaseProviderFactory
    {
        internal static DbProviderFactory? GetDbProviderFactory(string providerName)
        {
            return providerName switch
            {
                "Microsoft.Data.SqlClient" => SqlClientFactory.Instance,
                "PostgreSQL" => NpgsqlFactory.Instance, // Requires Npgsql package
                "MySQL" => MySqlClientFactory.Instance, // Requires MySql.Data package
                _ => throw new ArgumentException($"Unsupported database provider: {providerName}")
            };
        }
    }
}