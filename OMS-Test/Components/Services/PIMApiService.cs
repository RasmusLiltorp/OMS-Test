using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMS_Services;

/// <summary>
/// Service responsible for interacting with the PIM API.
/// </summary>
class PIMApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public async Task<ProductDTO?> GetProductAsync(string productId)
    {
        try
        {
            Console.WriteLine($"Fetching product {productId}...");
            return new ProductDTO(){
                ProductID = productId,
                ProductName = "test",
                Price = 123,
                Weight = (decimal)1.23,
            };
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
        Console.WriteLine("Fetching all products...");
        return new List<ProductDTO?>(){
            new(){
                ProductID = Guid.NewGuid().ToString(),
                ProductName = "test",
                Price = 123,
                Weight = (decimal)1.53,
            },
            new(){
                ProductID = Guid.NewGuid().ToString(),
                ProductName = "tes2",
                Price = 1234,
                Weight = (decimal)1.234,
            },
            new(){
                ProductID = Guid.NewGuid().ToString(),
                ProductName = "test3",
                Price = 1235,
                Weight = (decimal)1.235,
            }
            
        };
        /*try
        {
            int page = 10;
            var response = await _http.GetFromJsonAsync<List<ProductDTO>>($"api//products/list/{page}");
            return response;
        }
        catch
        {
            Console.WriteLine("Error fetching all products.");
            return null;
        }*/
    }
}