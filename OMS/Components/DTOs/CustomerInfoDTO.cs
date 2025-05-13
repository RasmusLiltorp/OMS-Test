using System.Text.Json.Serialization;
using DTOs;

public class CustomerInfoDTO
{
    [JsonPropertyName("customer-id")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    // This is a placeholder value.
    public string Email {get; set;} = "Placeholder@oms.dk";
}