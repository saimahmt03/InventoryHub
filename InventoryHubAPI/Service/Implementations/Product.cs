using System.Threading.Tasks;
using InventoryHubAPI.Service.Interfaces; 
using InventoryHubAPI.DTO.Request;
using InventoryHubAPI.Reporitory.Interfaces; 
using InventoryHubAPI.DTO.Result;
using InventoryHubAPI.Shared; 
namespace InventoryHubAPI.Service.Implementations
{
    internal class Product : IProduct
    {
        private readonly IRepository _repository; 
        public Product(IRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<BaseResult> AddProductAsync(ProductRequest productRequest)
        {
            BaseResult serviceResult = new BaseResult();
            BaseResult processorResult = new BaseResult();
            BaseResult repositoryResult = new BaseResult();
            Processor processor = new Processor();

            processorResult = processor.PriceChecker(productRequest); // Other logic.
            if(processorResult.Code == Result.ResultCode.Success)
            {
                repositoryResult = await _repository.AddProductAsync(productRequest);

                return repositoryResult;
            }
            else 
            {
                serviceResult.Code = processorResult.Code;
                serviceResult.Message = processorResult.Message; 
                return serviceResult;
            }
        }
        
        public async Task<BaseResult> UpdateProductAsync(ProductRequest productRequest)
        {
            Processor processor = new Processor();
            BaseResult serviceResult = new BaseResult();
            BaseResult repositoryResult = new BaseResult();
            BaseResult processorResult = new BaseResult();

            processorResult = processor.PriceChecker(productRequest); // Other logic.
            if(serviceResult.Code == Result.ResultCode.Success)
            {
                repositoryResult = await _repository.UpdateProductAsync(productRequest);

                return repositoryResult;
            } 
            else
            {
                serviceResult.Code = processorResult.Code;
                serviceResult.Message = processorResult.Message; 
                return serviceResult;
            }  
        }

        public async Task<BaseResult> RemoveProductAsync(ProductRequest productRequest)
        {
            BaseResult serviceResult = new BaseResult();
            BaseResult repositoryResult = new BaseResult();

            repositoryResult = await _repository.RemoveProductAsync(productRequest);
            if(repositoryResult.Code == Result.ResultCode.Success)
            {
                return repositoryResult;
            } 
            else
            {
                serviceResult.Code = repositoryResult.Code;
                serviceResult.Message = repositoryResult.Message;
                return serviceResult;
            }  
        }

        public async Task<GetAllProductResult> GetProductListAsync()
        {
            GetAllProductResult result = new GetAllProductResult();
            
            result = await _repository.GetProductListAsync();

            return result;
        }   
    }
}