using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

/// <summary>
/// Thiss class is responsible for managing the data layer of the application.
/// It interacts with the API service to fetch and manipulate order data.
/// </summary>
public class DataService
{
    private readonly ApiService _apiService;
    private readonly PIMApiService _pimApiService;
    public List<ProductDTO> Products { get; private set; } = new();
    private readonly SemaphoreSlim _dataInitLock = new SemaphoreSlim(1, 1);

    public List<OrderDTO> OrderLines { get; private set; } = new();
    public event EventHandler OrdersFetched = delegate { };

    public DataService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        var baseUrl = configuration["BackendSettings:BaseUrl"] ?? "http://localhost:4300/";
        
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(baseUrl);

        _apiService = new ApiService(httpClient);
        _pimApiService = new PIMApiService();
        
        // Initialize data
        _ = InitializeDataAsync();
    }

    public async Task InitializeDataAsync()
    {
        await _dataInitLock.WaitAsync();
        try
        {        
            var ordersFromApi = await _apiService.GetMultipleOrdersAsync(100); // Fetch 100 orders - simplfied for MVP
            if (ordersFromApi != null)
            {
                OrderLines = ConvertResponseToOrderLine(ordersFromApi);
            }
            else
            {
                Console.WriteLine("No orders found or error occurred while fetching orders.");
            }
            // Initilize all products
            var productsResult = await _pimApiService.GetAllProductsAsync();
            if (productsResult != null)
            {
                var nonNullProducts = productsResult
                    .Where(p => p != null)
                    .Cast<ProductDTO>()
                    .ToList();
                Products = nonNullProducts;
            }
            else
            {
                Products = new List<ProductDTO>();
            } 
        
        }
        finally
        {
            _dataInitLock.Release();
            OrdersFetched.Invoke(this, EventArgs.Empty);
        }
    }
    private List<OrderDTO> ConvertResponseToOrderLine(RootDTO apiResponses)
    {
        var result = new List<OrderDTO>();
        if (apiResponses?.Data == null)
        {
            return result;
        }
        
        foreach(var orderData in apiResponses.Data)
        {            
            var order = new OrderDTO
            {
                OrderId = orderData.OrderId,
                LineElements = orderData.LineElements != null ? 
                    new List<LineElementDTO>(orderData.LineElements) : 
                    new List<LineElementDTO>(),
                TotalCost = orderData.TotalCost,
                Date = orderData.Date,
                FulfillmentState = orderData.FulfillmentState,
                CustomerInfo = orderData.CustomerInfo,
                ShippingInfo = orderData.ShippingInfo
            };
            
            result.Add(order);
        }
        
        return result;
    }

    public async Task<bool> UpdateOrderAsync(OrderDTO order)
    {
        try {
            var patchData = new {
                customerInfo = new {
                    name = order.CustomerInfo.Name,
                    customerId = order.CustomerInfo.CustomerId
                },
                shippingInfo = new {
                    address1 = order.ShippingInfo.Address1,
                    address2 = order.ShippingInfo.Address2,
                    zipcodeId = order.ShippingInfo.ZipcodeId,
                    countryId = order.ShippingInfo.CountryId
                },
                lineElements = order.LineElements,
                totalCost = order.TotalCost,
                date = order.Date
            };
            
            var result = await _apiService.PatchOrderAsync(order.OrderId, patchData);
            return result?.Status == "Success";
        }
        catch (Exception ex) {
            Console.WriteLine($"Update order error: {ex.Message}");
            return false;
        }
    }

    public void SaveNewUniqueProduct(ProductDTO product)
    {
    }
    public void ProductFromOrder()
    {
    }
    public decimal CalculateOrdertotal(OrderDTO order)
    {
        decimal total = 0;
        foreach (var lineElement in order.LineElements)
        {
            total += lineElement.Price * lineElement.Amount;
        }
        return total;
    }
    public decimal CalculateWeight()
    {
        return 0;
    }

    public async Task<List<BrandAnalyticsDTO>> GetBrandAnalyticsAsync(DateOnly from, DateOnly to)
    {
        var brands = await _apiService.GetBrandAnalyticsAsync(from, to);
        return brands ?? new List<BrandAnalyticsDTO>();
    }

    public async Task<List<CategoryAnalyticsDTO>> GetCategoryAnalyticsAsync(DateOnly from, DateOnly to)
    {
        var categories = await _apiService.GetCategoryAnalyticsAsync(from, to);
        return categories ?? new List<CategoryAnalyticsDTO>();
    }

    public async Task<List<string>> GetAllBrandsAsync()
{
    var brandAnalytics = await _apiService.GetBrandAnalyticsAsync(DateOnly.MinValue, DateOnly.MaxValue);
    return brandAnalytics?.Select(b => b.BrandName).Distinct().ToList() ?? new List<string>();
}

public async Task<List<string>> GetAllCategoriesAsync()
{
    var categoryAnalytics = await _apiService.GetCategoryAnalyticsAsync(DateOnly.MinValue, DateOnly.MaxValue);
    return categoryAnalytics?.Select(c => c.CategoryName).Distinct().ToList() ?? new List<string>();
}


}
