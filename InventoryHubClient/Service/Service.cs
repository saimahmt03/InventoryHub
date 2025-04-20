using System.Net.Http.Json;
using System.Net;
using Blazored.SessionStorage;
using InventoryHubClient.Common;
using InventoryHubClient.DTO;
using InventoryHubClient.Service.Storage;

namespace InventoryHubClient.Service
{
    internal class Service : IService
    {
        private Product _currentProduct = new Product();
        private ProductList _productList = new ProductList();

        private const string ProductListCacheKey = "CachedProductList";

        private readonly IResponseStatusHandler _responseStatusHandler = new ResponseStatusHandler();
        private readonly HttpClient _inventoryHubClient;
        private readonly ISessionStorage _sessionStorage;

        public Service(IHttpClientFactory clientFactory, IResponseStatusHandler responseStatusHandler, ISessionStorage sessionStorage)
        {
            // This prevent socket exhaustion issue.
            // The HttpClient is created and managed by the factory, which handles the underlying socket connections.
            _inventoryHubClient = clientFactory.CreateClient("inventoryhubapi"); // This is the name of the HttpClient/API registered in Program.cs

            _responseStatusHandler = responseStatusHandler;
            _sessionStorage = sessionStorage;
        }

        public event Func<Task>? StateChanged;

        
        #region Mainly use for Product process.
        public async Task<BaseResponse> AddProductAsync(Product newproduct)
        {
            BaseResponse baseResponse = new BaseResponse();
            try
            {
                // Traditional way of Serialization or Manual creating JSON
                // var json = JsonSerializer.Serialize(product);
                // var content = new StringContent(json, Encoding.UTF8, "application/json");
                // var response = await _http.PostAsync("api/products", content); // Any content such as file, xml, form, etc.

                // Modern way of Serialization. Exclusively for JSON
                // API call to add a new product
                var response = await _inventoryHubClient.PostAsJsonAsync("addproduct", newproduct);
               
                _responseStatusHandler.HandleResponseStatus(response, baseResponse);
                
                if(response.StatusCode == HttpStatusCode.Created)
                {
                    // Add the new product to the list 
                    _productList.Products.Add(newproduct); 

                    // Add the cached product list in session storage
                    await _sessionStorage.SetItemAsync("CachedProductList", _productList);

                    await NotifyStateChangedAsync(); 
                }
            }
            catch (HttpRequestException ex)
            {
                // This is for network-related errors (e.g., no internet connection, API down, etc.)
                string err = $"Error adding product: {ex.Message}"; // For logging purposes
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            catch (Exception ex)
            {
                // This will catch other types of unexpected exceptions
                string err = $"Unexpected error: {ex.Message}"; // For logging purposes
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            return baseResponse;
        }

        public async Task<BaseResponse> UpdateProductAsync(Product product)
        {
            BaseResponse baseResponse = new BaseResponse();
            try
            {
                // API call to update the product
                var response = await _inventoryHubClient.PutAsJsonAsync("updateproduct", product);
                
                _responseStatusHandler.HandleResponseStatus(response, baseResponse);
                
                if(response.StatusCode == HttpStatusCode.NoContent)
                {   
                    // Remove old product from list
                    _productList.Products.Remove(_productList.Products.First(p => p.Id == product.Id));
                    
                    // Add updated product to list
                    _productList.Products.Add(product);
                    
                    // Update the cached product list in session storage
                    await _sessionStorage.SetItemAsync("CachedProductList", _productList);
                    
                    await NotifyStateChangedAsync(); 
                }
            }
            catch (HttpRequestException ex)
            {
                string err = $"Error updating product: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            catch (Exception ex)
            {
                string err = $"Unexpected error: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            return baseResponse;
        }

        public async Task<BaseResponse> RemoveProductAsync(Product product)
        {
            BaseResponse baseResponse = new BaseResponse();
            try
            {
                // API call to remove the product
                var response = await _inventoryHubClient.DeleteAsync("removeproduct", product); 
                
                _responseStatusHandler.HandleResponseStatus(response, baseResponse);
                
                if(response.StatusCode == HttpStatusCode.NoContent)
                {
                    // Remove product from list
                    _productList.Products.Remove(_productList.Products.First(p => p.Id == product.Id));

                    // Update the cached product list in session storage
                    await _sessionStorage.SetItemAsync("CachedProductList", _productList);

                    await NotifyStateChangedAsync(); 
                }
            }
            catch (HttpRequestException ex)
            {
                string err = $"Error removing product: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            catch (Exception ex)
            {
                string err = $"Unexpected error: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            return baseResponse;
        }

        public async Task<BaseResponse> SearchProductsAsync(string productName)
        {
            BaseResponse baseResponse = new BaseResponse();
            try
            {
                var response = await _inventoryHubClient.PostAsJsonAsync($"searchproduct", productName);
                
                _responseStatusHandler.HandleResponseStatus(response, baseResponse);

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<ProductList>(); //?? new ProductList(); // Modern way.
                    // Traditional way. This is equal to ?? new ProductList() to prevent null value.
                    if (responseContent != null)
                    {
                        _productList = responseContent;

                        await _sessionStorage.SetItemAsync(ProductListCacheKey, _productList);
                    }
                    else
                    {
                        _productList = new ProductList();
                    }
                    await NotifyStateChangedAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                string err = $"Error searching product: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            catch (Exception ex)
            {
                string err = $"Unexpected error: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            return baseResponse;
        }

        public async Task<ProductList> GetProductListAsync()
        {
            ProductList baseResponse = new ProductList();
            try
            {
                // Cache the product list 
                var cachedProductList = await _sessionStorage.GetItemAsync<ProductList>(ProductListCacheKey);
                if (cachedProductList != null && cachedProductList.Products.Count > 0)
                {
                    _productList = cachedProductList;
                    await NotifyStateChangedAsync();
                }    

                // API call to get the product list
                var response = await _inventoryHubClient.GetAsync("getproductlist");
                
                _responseStatusHandler.HandleResponseStatus(response, baseResponse);
                
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<ProductList>();
                    if (responseContent != null)
                    {
                        _productList = responseContent;
                        // Cache the product list in session storage
                        await _sessionStorage.SetItemAsync(ProductListCacheKey, _productList);
                    }
                    else
                    {
                        _productList = new ProductList();
                    }
                    await NotifyStateChangedAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                string err = $"Error retrieving products: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            catch (Exception ex)
            {
                string err = $"Unexpected error: {ex.Message}";
                baseResponse.Code = Response.ResponseCode.Error;
                baseResponse.Message = Response.ResponseMessage.Error;
            }
            return baseResponse;
        }
        #endregion
        
        public async Task NotifyStateChangedAsync()
        {
            if (StateChanged != null)
            {
                await StateChanged.Invoke();
            }
            await Task.Yield(); // Ensures async execution without blocking UI
        }   
    }
}