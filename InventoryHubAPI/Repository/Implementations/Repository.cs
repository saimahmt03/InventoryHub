using System.Data.Common;
using System.Data;
using InventoryHubAPI.Data;
using InventoryHubAPI.DTO.Request;
using InventoryHubAPI.DTO.Result;
using InventoryHubAPI.Reporitory.Interfaces;
using InventoryHubAPI.Shared;

namespace InventoryHubAPI.Reporitory.Implementations
{
    internal class Repository : IRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        public Repository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<BaseResult> AddProductAsync(ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Creating and opening database connection.
                using DbConnection connection = await _databaseConnection.CreateConnectionAsync();

                // Creating command and telling the system what type of command we're using which is stored procedure.
                using DbCommand command = connection.CreateCommand();
                command.CommandText = StoredProcedures.AddProduct;
                command.CommandType = CommandType.StoredProcedure;
                
                // This will check if the database provider is valid. If valid then happy path, if not then throw an error.
                DbProviderFactory? factory;
                string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                if(!string.IsNullOrEmpty(errorMessage))
                {
                    result.Code = Result.ResultCode.Error;
                    //result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
                    result.Message = String.Format("{0} (Try -- Something went wrong.)", Result.ResultMessage.Error); // For testing only
                }
                else
                {
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Name", productRequest.Name, DbType.String, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Price", productRequest.Price, DbType.Double, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Stock", productRequest.Stock, DbType.Int32, factory!));

                    await command.ExecuteNonQueryAsync();
                    result.Code = Result.ResultCode.Success;
                    result.Message = String.Format("{0} (Product )", Result.ResultMessage.Success);
                }
                  
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Adding product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                //result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
                result.Message = String.Format("{0} (DB Ex -- Something went wrong.)", logMessage); // For testing only
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("General error in Adding product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                //result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
                result.Message = String.Format("{0} (Ex -- Something went wrong.)", logMessage); // For testing only
            }
            return result;
        }

        public async Task<BaseResult> UpdateProductAsync(ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();
            try
            {
                using DbConnection connection = await _databaseConnection.CreateConnectionAsync();

                using DbCommand command = connection.CreateCommand();
                command.CommandText = StoredProcedures.UpdateProduct;
                command.CommandType = CommandType.StoredProcedure;

                DbProviderFactory? factory;
                string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                if(!string.IsNullOrEmpty(errorMessage))
                {
                    result.Code = Result.ResultCode.Error;
                    result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
                }
                else
                {
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Id", productRequest.Id, DbType.Int32, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Name", productRequest.Name, DbType.String, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Price", productRequest.Price, DbType.Double, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Stock", productRequest.Stock, DbType.Int32, factory!));

                    await command.ExecuteNonQueryAsync();
                    result.Code = Result.ResultCode.Success;
                    result.Message = String.Format("{0} (Update product )", Result.ResultMessage.Success);   
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Updating product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("General error in Updating product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            return result;
        }

        public async Task<BaseResult> RemoveProductAsync(ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();
            try
            {
                using DbConnection connection = await _databaseConnection.CreateConnectionAsync();

                using DbCommand command = connection.CreateCommand();
                command.CommandText = StoredProcedures.RemoveProduct;
                command.CommandType = CommandType.StoredProcedure;

                DbProviderFactory? factory;
                string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                if(!string.IsNullOrEmpty(errorMessage))
                {
                    result.Code = Result.ResultCode.Error;
                    result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
                }
                else
                {
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Id", productRequest.Id, DbType.Int32, factory!));
                    await command.ExecuteNonQueryAsync();
                    result.Code = Result.ResultCode.Success;
                    result.Message = String.Format("{0} (Remove product )", Result.ResultMessage.Success);   
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Removing product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("General error in Removing product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            return result;
        }

        public async Task<GetAllProductResult> GetProductListAsync()
        {
            GetAllProductResult result = new GetAllProductResult();
            try
            {
                using DbConnection connection = await _databaseConnection.CreateConnectionAsync();

                using DbCommand command = connection.CreateCommand();
                command.CommandText = StoredProcedures.GetAllProducts;
                command.CommandType = CommandType.StoredProcedure;

                DbProviderFactory? factory;
                string errorMessage = _databaseConnection.TryGetDbProviderFactory(out factory);

                if(!string.IsNullOrEmpty(errorMessage))
                {
                    result.Code = Result.ResultCode.Error;
                    result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
                }
                else
                {
                    using DbDataReader reader = command.ExecuteReader();
                    ProductRequest product = new ProductRequest
                    {
                        Name = reader["Name"].ToString() ?? string.Empty,
                        Price = Convert.ToDouble(reader["Price"].ToString()),
                        Stock = Convert.ToInt32(reader["Stock"].ToString())
                    };
                    result.ProductList.Add(product);
                    result.Code = Result.ResultCode.Success;
                    result.Message = Result.ResultMessage.Success;
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Get all product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("General error in Get all product:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            return result;
        }
    }
}
