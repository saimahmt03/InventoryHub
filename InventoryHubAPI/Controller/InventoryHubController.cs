using Microsoft.AspNetCore.Mvc;
using InventoryHubAPI.DTO.Request;
using InventoryHubAPI.Reporitory.Interfaces;
using InventoryHubAPI.DTO.Result;
using InventoryHubAPI.Service.Interfaces;
using InventoryHubAPI.Shared;

namespace InventoryHubAPI.Controller
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("inventoryhubapi")]
    public class InventoryHubController : ControllerBase
    {
        private readonly IProduct _productService;
        public InventoryHubController(IProduct productService)
        {
            _productService = productService;
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                result = await _productService.AddProductAsync(productRequest);
                if(result.Code == Result.ResultCode.Success)
                {
                    return Ok(result);
                }
                else if(result.Code == Result.ResultCode.Error)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }  
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                result = await _productService.UpdateProductAsync(productRequest);
                if(result.Code == Result.ResultCode.Success)
                {
                    return Ok(result);
                }
                else if(result.Code == Result.ResultCode.Error)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }  
                else
                {
                    return BadRequest(result);
                }
            }
        }   
    
        [HttpDelete("removeproduct")]
        public async Task<IActionResult> RemoveProduct([FromBody] ProductRequest productRequest)
        {
            BaseResult result = new BaseResult();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                result = await _productService.RemoveProductAsync(productRequest);
                if(result.Code == Result.ResultCode.Success)
                {
                    return Ok(result);
                }
                else if(result.Code == Result.ResultCode.Error)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }  
                else
                {
                    return BadRequest(result);
                }
            }
        }

        [HttpGet("getproductlist")]
        public async Task<IActionResult> GetProductList()
        {
            GetAllProductResult result = new GetAllProductResult();
            
            result = await _productService.GetProductListAsync();
            if(result.Code == Result.ResultCode.Success)
            {
                return Ok(result);
            }
            else if(result.Code == Result.ResultCode.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }  
            else
            {
                return BadRequest(result);
            }
        }
    }
}