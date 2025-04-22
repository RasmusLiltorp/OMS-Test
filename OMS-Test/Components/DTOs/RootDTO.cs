using System.Text.Json.Serialization;
namespace DTOs;

public class RootDTO
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public List<OrderDTO> Data { get; set; }
}
