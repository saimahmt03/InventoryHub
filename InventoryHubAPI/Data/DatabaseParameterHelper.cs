using System.Data;
using System.Data.Common;

namespace InventoryHubAPI.Data
{
    internal static class DatabaseParameterHelper
    {
        public static DbParameter CreateParameter(string name, object value, DbType type, DbProviderFactory factory)
        {
            DbParameter parameter = factory.CreateParameter() 
                ?? throw new InvalidOperationException("Failed to create a database parameter.");
            
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value ?? DBNull.Value; // Handle null values properly

            return parameter;
        }
    }
}