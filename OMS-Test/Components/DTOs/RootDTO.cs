using System.Text.Json.Serialization;
namespace DTOs;

public class RootDTO
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public List<OrderDataDTO> Data { get; set; }
}

public class OrderDataDTO
{
    [JsonPropertyName("order-id")]
    public string OrderId { get; set; }

    [JsonPropertyName("line-elements")]
    public List<LineElementDTO> LineElements { get; set; }

    [JsonPropertyName("total-cost")]
    public decimal TotalCost { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("fulfillment-state")]
    public int FulfillmentState { get; set; }

    [JsonPropertyName("customer-info")]
    public CustomerInfoDTO CustomerInfo { get; set; }

    [JsonPropertyName("shipping-info")]
    public ShippingInfoDTO ShippingInfo { get; set; }
}
public class LineElementDTO
{
    [JsonPropertyName("product-uuid")]
    public int ProductUuid { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}

public class CustomerInfoDTO
{
    [JsonPropertyName("customer-id")]
    public string CustomerId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
public class ShippingInfoDTO
{
    [JsonPropertyName("address1")]
    public string Address1 { get; set; }

    [JsonPropertyName("address2")]
    public string Address2 { get; set; }

    [JsonPropertyName("zipcode-id")]
    public int ZipcodeId { get; set; }

    [JsonPropertyName("country-id")]
    public int CountryId { get; set; }
}
