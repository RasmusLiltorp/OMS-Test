using System.Text.Json.Serialization;
namespace DTOs;

public class OrderDTO
{
    [JsonPropertyName("order-id")]
    public string OrderId { get; set; } = string.Empty;

    [JsonPropertyName("line-elements")]
    public List<LineElementDTO> LineElements { get; set; } = new();

    [JsonPropertyName("total-cost")]
    public decimal TotalCost { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("fulfillment-state")]
    public int FulfillmentState { get; set; }

    [JsonPropertyName("customer-info")]
    public CustomerInfoDTO CustomerInfo { get; set; } = new();

    [JsonPropertyName("shipping-info")]
    public ShippingInfoDTO ShippingInfo { get; set; } = new();
}
