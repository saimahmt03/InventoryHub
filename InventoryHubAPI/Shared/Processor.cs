using InventoryHubAPI.DTO.Request;
using InventoryHubAPI.DTO.Result;

namespace InventoryHubAPI.Shared
{
    internal class Processor
    {
        public BaseResult PriceChecker(ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();
            List<int> Prices = new List<int>() {15, 25, 35, 45, 50};

            foreach (var price in Prices)
            {
                if(productRequest.Price == price)
                {
                    result.Code = Result.ResultCode.Success;
                    result.Message = Result.ResultMessage.Success;  
                }
                else
                {
                    result.Code = Result.ResultCode.Invalid;
                    result.Message = String.Format("{0} (Price is not on the list.)",Result.ResultMessage.Invalid);
                }
            }
            return result;            
        }
    }
}