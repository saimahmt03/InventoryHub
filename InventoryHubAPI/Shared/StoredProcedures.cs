

namespace InventoryHubAPI.Shared
{
    internal static class StoredProcedures
    {
        public const string AddProduct = "[dbo].[AddProduct]";
        public const string UpdateProduct = "[dbo].[UpdateProduct]";
        public const string GetAllProducts = "[dbo].[GetAllProduct]";
        public const string GetProductById = "";
        public const string RemoveProduct = "";


        #region Logging
        public const string LogRequest = "[dbo].[LogRequest]";
        public const string LogResponse = "[dbo].[LogResponse]";
        #endregion
    }
}