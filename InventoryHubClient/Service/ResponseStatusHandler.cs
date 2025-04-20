using System.Net.Http.Json;
using System.Net;
using InventoryHubClient.Common;
using InventoryHubClient.DTO;

namespace InventoryHubClient.Service
{
    internal class ResponseStatusHandler : IResponseStatusHandler
    {
        
        public void HandleResponseStatus(HttpResponseMessage response, BaseResponse baseResponse)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    baseResponse.Code = Response.ResponseCode.Success;
                    baseResponse.Message = Response.ResponseMessage.Success;
                    break;
                case HttpStatusCode.NoContent:
                    baseResponse.Code = Response.ResponseCode.Updated;
                    baseResponse.Message = Response.ResponseMessage.Updated;
                    break;
                case HttpStatusCode.OK:
                    baseResponse.Code = Response.ResponseCode.Retrieved;
                    baseResponse.Message = Response.ResponseMessage.Retrieved;
                    break;
                case HttpStatusCode.BadRequest:
                    baseResponse.Code = Response.ResponseCode.Invalid;
                    baseResponse.Message = Response.ResponseMessage.Invalid;
                    break;
                default:
                    baseResponse.Code = Response.ResponseCode.Error;
                    baseResponse.Message = Response.ResponseMessage.Error;
                    break;
            }
        }
    }
}