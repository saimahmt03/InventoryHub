using System.Data.Common;
using System.Data;
using InventoryHubAPI.Data;
using InventoryHubAPI.Reporitory.Interfaces;
using InventoryHubAPI.Shared;
using InventoryHubAPI.DTO.Result;

namespace InventoryHubAPI.Reporitory.Implementations
{
    internal class LoggerRepository : ILoggerRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        public LoggerRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task LogRequestAsync(string method, string path, string body)
        {
            BaseResult result = new BaseResult();
            try
            {
                using DbConnection connection = await _databaseConnection.CreateConnectionAsync();

                using DbCommand command = connection.CreateCommand();
                command.CommandText = StoredProcedures.LogRequest;
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
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Method", method, DbType.String, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Path", path, DbType.String, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Body", body, DbType.String, factory!));

                    await command.ExecuteNonQueryAsync();
                    result.Code = Result.ResultCode.Success;
                    result.Message = String.Format("{0} (Request Log )", Result.ResultMessage.Success);   
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Request logging:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("General error in Request logging:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            finally
            {
                await Task.CompletedTask;
            }
        }

        public async Task LogResponseAsync(string method, string path, int statusCode, string body)
        {
            BaseResult result = new BaseResult();
            try
            {
                using DbConnection connection = await _databaseConnection.CreateConnectionAsync();

                using DbCommand command = connection.CreateCommand();
                command.CommandText = StoredProcedures.LogResponse;
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
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Method", method, DbType.String, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Path", path, DbType.String, factory!));
                    command.Parameters.Add(DatabaseParameterHelper.CreateParameter("@Body", body, DbType.String, factory!));

                    await command.ExecuteNonQueryAsync();
                    result.Code = Result.ResultCode.Success;
                    result.Message = String.Format("{0} (Response Log )", Result.ResultMessage.Success);   
                }
            }
            catch (DbException ex)
            {
                string logMessage = string.Concat("Database error in Response logging:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            catch (Exception ex)
            {
                string logMessage = string.Concat("General error in Response logging:", " ", ex.Message);
                //_logger.LogError(ex, logMessage);
                result.Code = Result.ResultCode.Error;
                result.Message = String.Format("{0} (Something went wrong.)", Result.ResultMessage.Error);
            }
            finally
            {
                await Task.CompletedTask;
            }
        }
    }
}