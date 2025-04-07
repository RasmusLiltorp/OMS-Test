using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using DTOs;

namespace OMS_Services;

public class DataService
{
    private readonly ApiService _apiService;

    public List<ProductDTO> Products { get; private set; } = new();
    public List<OrderLine> OrderLines { get; private set; } = new();

    public DataService(ApiService apiService)
    {
        _apiService = apiService;
        InitializeDataAsync();
    }

    private async void InitializeDataAsync()
    {
        var ordersFromApi = await _apiService.GetMultipleOrdersAsync(100);
        if (ordersFromApi.Status == "Success" && ordersFromApi.Data != null)
        {
            OrderLines = ConvertResponseToOrderLine(ordersFromApi.Data);
        }
        else
        {
            Console.WriteLine("No orders found or error occurred while fetching orders.");
        }
    }

    private List<OrderLine> ConvertResponseToOrderLine(List<JsonDTO.OrderDataDTO> apiResponse)
    {
        var result = new List<OrderLine>();
        foreach (var order in apiResponse)
        {
            var orderLine = new OrderLine
            {
                OrderId = order.OrderId,
                Customer = order.CustomerInfo == null ? "" : order.CustomerInfo.Name,
                OrderDate = order.Date,
                TrackAndTrace = null,
                Products = order.LineElements == null
                    ? new List<OrderProduct>()
                    : order.LineElements.ConvertAll(le => new OrderProduct
                    {
                        ProductID = le.ProductUuid.ToString(),
                        Quantity = le.Amount,
                        Price = le.Price
                    })
            };
            result.Add(orderLine);
        }
        return result;
    }

    public void SaveNewUniqueProduct(ProductDTO product)
    {
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
