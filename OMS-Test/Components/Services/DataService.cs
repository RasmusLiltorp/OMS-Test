using System.Net.Http;
using System.Net.Http.Json;
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
    public List<ProductDTO> Products { get; private set; }


    public List<OrderDTO> OrderLines { get; private set; } = new();

    public DataService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        var baseUrl = configuration["BackendSettings:BaseUrl"] ?? "http://localhost:4300/";
        
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(baseUrl);

        _apiService = new ApiService(httpClient);
       // _pimApiService = new PIMApiService(httpClient);
        
        InitializeDataAsync();
    }

    private async void InitializeDataAsync()
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
                LineElements = orderData.LineElements,
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
}
