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


    public List<LineElementDTO> OrderLines { get; private set; } = new();

    public DataService(ApiService apiService)
    {
        _apiService = new ApiService(new HttpClient());    
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

    private List<LineElementDTO> ConvertResponseToOrderLine(RootDTO apiResponses)
    {
        var result = new List<LineElementDTO>();
        if (apiResponses?.Data == null)
        {
            return result;
        }

        foreach (var order in apiResponses.Data)
        {
            if (order?.LineElements == null)
            {
                continue;
            }
            foreach (var line in order.LineElements)
            {
                result.Add(new LineElementDTO
                {
                    ProductUuid = line.ProductUuid,
                    Amount = line.Amount,
                    Price = line.Price
                });
            }
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
        return 0;
    }
    public decimal CalculateWeight()
    {
        return 0;
    }
}
