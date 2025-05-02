using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

/// <summary>
/// Service responsible for interacting with the PIM API.
/// </summary>
class PIMApiService
{
    private readonly HttpClient _http = new();
    // private readonly JsonSerializerOptions _jsonOptions = new();

    // Temporary mock data
    public Task<List<ProductDTO?>> GetAllProductsAsync() =>
        Task.FromResult(new List<ProductDTO?>
        {
            new() { ProductID="product-1", ProductName="test", Price=123, Weight=1.53m, BrandName="brand1", ProductCategory="cat1" },
            new() { ProductID="product-2", ProductName="test2", Price=1234, Weight=1.234m, BrandName="brand2", ProductCategory="cat2" },
            new() { ProductID="product-3", ProductName="test3", Price=1235, Weight=1.235m, BrandName="brand3", ProductCategory="cat1" }
        });
}

/*
     public async Task<ProductDTO?> GetProductAsync(string productId)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ProductDTO>($"api/products/{productId}");
            if (response == null)
            {
                Console.WriteLine($"Error fetching product {productId}.");
                return null;
         
            }
            return new ProductDTO
            {
                ProductID = response.ProductID,
                ProductName = response.ProductName,
                Price = response.Price,
                Weight = 10 // Example weight
            };
        }
        catch
        {
            Console.WriteLine($"Error fetching product {productId}.");
            return null;
        }

    }

    public async Task<List<ProductDTO?>> GetAllProductsAsync()
    {
        try
        {
            int page = 10;
            var response = await _http.GetFromJsonAsync<List<ProductDTO>>($"api//products/list/{page}");
            return response;
        }
        catch
        {
            Console.WriteLine("Error fetching all products.");
            return new();
        }
    }
    */