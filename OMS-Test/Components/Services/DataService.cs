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
        _apiService = ApiService;
        InitializeDataAsync().GetAwaiter().GetResult();
    }

    private void InitializeDataAsync()
    {
        var ordersFromApi = await _apiService.GetMultipleOrdersAsync(100); // Fetch 100 orders - simplfied for MVP
        if(ordersFromApi != null && ordersFromApi.Any())
        {
            OrderLines = ConvertResponseToOrderLine(ordersFromApi);
        }
        else
        {
            Console.WriteLine("No orders found or error occurred while fetching orders.");
        }
    }

    private List<OrderLine> ConvertResponseToOrderLine(ApiResponse<Order>[] apiResponses)
    {
        List<OrderLine> result = new List<OrderLine>();
        foreach(var response in apiResponses)
        {
            if(response.Status != "201" || response.Data == null)
            {
                Console.WriteLine($"Sucess: {response.Status} - {response.Message}");
                continue;
            }
            var order = response.Data;
            var orderLine = new OrderLine
            {
                OrderId = order.OrderId.ToString(),
                Customer = order.CustomerInfo.Name,
                Email = "no-email-asigned@gmail.com", //placeholder
                OrderDate = DateTime.Parse(order.Date, out var date)
                TrackAndTrace = "12345678", //placeholder
            };
        }
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
    }
    public decimal CalculateWeight()
    {
    }
}
