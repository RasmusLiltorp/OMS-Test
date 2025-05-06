using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DTOs;

public class ProductDetailDTO
{
    [JsonPropertyName("product_id")]
    public string ProductId { get; set; } = string.Empty;
    
    [JsonPropertyName("parent_id")]
    public string ParentId { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
    
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    
    [JsonPropertyName("stock_count")]
    public int StockCount { get; set; }
    
    [JsonPropertyName("image_identifier")]
    public string ImageIdentifier { get; set; } = string.Empty;
    
    [JsonPropertyName("attributes")]
    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    
    [JsonPropertyName("variants")]
    public List<ProductVariantDTO> Variants { get; set; } = new List<ProductVariantDTO>();
}

public class ProductVariantDTO
{
    [JsonPropertyName("product_id")]
    public string ProductId { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    
    [JsonPropertyName("stock_count")]
    public int StockCount { get; set; }
    
    [JsonPropertyName("image_identifier")]
    public string ImageIdentifier { get; set; } = string.Empty;
}