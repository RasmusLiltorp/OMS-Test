using System.Text.Json.Serialization;
using DTOs;

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

    // This is a placeholder value. 
    // In a real-world scenario, this would be dynamically generated or retrieved from a database.
    public string TrackingNumber { get; set; } = "12345678";
}
