using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

public class DataService
{
    private readonly ApiService _apiService;

    public List<ProductDTO> Products { get; private set; } = new();
    public List<OrderLine> OrderLines { get; private set; } = new();

    public DataService(ApiService apiService)
    {
        _apiService = new ApiService(new HttpClient());
        InitializeDataAsync();
    }

    private async void InitializeDataAsync()
    {
        var ordersFromApi = await _apiService.GetMultipleOrdersAsync(100); // Fetch 100 latest orders - simplfied for MVP
        if(ordersFromApi != null && ordersFromApi.Status == "Success" && ordersFromApi.Data != null)
        {
            OrderLines = ConvertResponseToOrderLine(ordersFromApi.Data);
        }
        else
        {
            Console.WriteLine("No orders found or error occurred while fetching orders.");
        }
    }

    private List<OrderLine> ConvertResponseToOrderLine(List<OrderLine> apiResponse)
    {
        List<OrderLine> result = new List<OrderLine>();
        foreach (var order in apiResponse)
        {
            var orderLine = new OrderLine
            {
                OrderId = order.OrderId,
                Customer = order.Customer,
                OrderDate = order.OrderDate,
                TrackAndTrace = order.TrackAndTrace,
                Products = order.Products
            };
            result.Add(orderLine);
        }
        return result;
    }
    public void SaveNewUniqueProduct(ProductDTO product)
    {
        //save product to list and grab productinfo from PIM
    }
    public void ProductFromOrder()
    {
    }
    public decimal CalculateOrdertotal()
    {
        return 1;
    }
    public decimal CalculateWeight()
    {
        return 1;
    }
}
