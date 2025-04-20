using InventoryHubClient.Common;
using InventoryHubClient.DTO;

namespace InventoryHubClient.Service
{
    public interface IResponseStatusHandler
    {
        void HandleResponseStatus(HttpResponseMessage response, BaseResponse baseResponse);
    }
}