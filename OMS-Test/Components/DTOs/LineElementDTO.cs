using System.Text.Json.Serialization;
using DTOs;

public class LineElementDTO
{
    [JsonPropertyName("product-uuid")]
    public int ProductUuid { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}