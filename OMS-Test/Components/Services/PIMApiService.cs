using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

/// <summary>
/// Service responsible for interacting with the PIM API.
/// </summary>
class PIMApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly string _baseUrl = "http://localhost:8080";

    public PIMApiService()
    {
        _http = new HttpClient();
        _http.BaseAddress = new Uri(_baseUrl);
    }

    /// <summary>
    /// Gets a detailed product by its ID
    /// </summary>
    public async Task<ProductDetailDTO?> GetProductDetailAsync(string productId)
    {
        try
        {
            var response = await _http.GetAsync($"api/products/{productId}");
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching product {productId}: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var productDetail = JsonSerializer.Deserialize<ProductDetailDTO>(content, _jsonOptions);
            
            return productDetail;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when fetching product {productId}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets a paginated list of all products with complete information.
    /// </summary>
    public async Task<List<ProductDTO>> GetAllProductsAsync(int page = 1, int pageSize = 50)
    {
        try
        {
            // Get basic list of products
            var response = await _http.GetAsync($"api/products/list?page={page}&page-size={pageSize}");
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching products: {response.StatusCode}");
                return new List<ProductDTO>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var productList = JsonSerializer.Deserialize<ProductListDTO>(content, _jsonOptions);

            if (productList == null || productList.Items == null || !productList.Items.Any())
            {
                return new List<ProductDTO>();
            }

            List<ProductDTO> result = new List<ProductDTO>();
            
            foreach (var item in productList.Items)
            {
                var detailedProduct = await GetProductDetailAsync(item.ProductId);
                
                if (detailedProduct != null)
                {
                    // Extract brand from attributes if it exists
                    string brandName = string.Empty;
                    if (detailedProduct.Attributes != null && 
                        detailedProduct.Attributes.TryGetValue("brand", out string? brand))
                    {
                        brandName = brand;
                    }
                    
                    result.Add(new ProductDTO
                    {
                        ProductID = detailedProduct.ProductId,
                        ProductName = detailedProduct.Name,
                        Price = detailedProduct.Price,
                        BrandName = brandName,
                        ProductCategory = detailedProduct.Category,
                        Weight = 0 
                    });
                }
                else
                {
                    result.Add(new ProductDTO
                    {
                        ProductID = item.ProductId,
                        ProductName = item.Name,
                        Price = 0,
                        BrandName = string.Empty,
                        ProductCategory = string.Empty,
                        Weight = 0
                    });
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception when fetching products: {ex.Message}");
            return new List<ProductDTO>();
        }
    }
}