using System.Text.Json.Serialization;
namespace DTOs;

public class ProductDTO
{
    [JsonPropertyName("product_id")]
    public required string ProductID { get; set; }
    [JsonPropertyName("name")]
    public required string ProductName { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    public string? BrandName { get; set; }
    public string? ProductCategory  { get; set; }
    
    //Placeholder
    public decimal Weight { get; set; }
}