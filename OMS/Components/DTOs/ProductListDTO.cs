using System.Text.Json.Serialization;
namespace DTOs;

public class ProductListDTO
{
    [JsonPropertyName("items")]
    public List<ProductListItemDTO> Items { get; set; } = new();

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("number_of_pages")]
    public int NumberOfPages { get; set; }
}

public class ProductListItemDTO
{
    [JsonPropertyName("product_id")]
    public string ProductId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}