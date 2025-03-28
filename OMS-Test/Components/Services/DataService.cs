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

    public DataService()
    {
        _apiService = ApiService;
        InitializeData();
    }

    private void InitializeData()
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

    private List<OrderLine> ConvertResponseToOrderLine(OrderLine[] apiOrders)
    {
        List<OrderLine> result = new List<OrderLine>();
        foreach (var order in apiOrders)
        {
            OrderLine orderLine = new OrderLine
            {
                Customer = order.Customer,
                Email = order.Email,
                Products = order.Products,
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TrackAndTrace = order.TrackAndTrace
            };
            result.Add(orderLine);
        }
        return result;
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
