using System.Net.Http;
using System.Text;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

/// <summary>
/// This class is responsible for making API calls to the backend.
/// </summary>
public class ApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient http)
    {
        _http = http;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Gets multiple orders from the backend.
    /// </summary>
    public async Task<RootDTO?> GetMultipleOrdersAsync(int amount)
    {
        try
        {            
            var response = await _http.GetFromJsonAsync<RootDTO>(
                $"api/order/?amount={amount}",
                _jsonOptions
            );
            if (response == null || response.Data == null || response.Status != "Success")
            {
                Console.WriteLine("Error: No orders found or error occurred while fetching orders.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = new()
                };
            }
            return response;
        }
        catch
        {
            Console.WriteLine("Error: No orders found or error occurred while fetching orders.");
            return new RootDTO
            {
                Status = "Exception",
                Data = new()
            };
        }
    }

    /// <summary>
    /// Patches an order with the given orderId and patchData. 
    /// "patchData" should be a JSON object with the properties to be updated.
    /// Example usage: "var patchData = new { fulfillment_state = 2 };"
    /// </summary>
    public async Task<RootDTO?> PatchOrderAsync(string orderId, object patchData)
    {
        try
        {
            var options = new JsonSerializerOptions(_jsonOptions)
            {
                PropertyNamingPolicy = new KebabCaseNamingPolicy(),
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            
            var jsonContent = JsonSerializer.Serialize(patchData, options);
            
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _http.PatchAsync($"api/order/{orderId}", content);
            
            response.EnsureSuccessStatusCode();
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            using (JsonDocument doc = JsonDocument.Parse(responseString))
            {
                string status = doc.RootElement.GetProperty("status").GetString() ?? "Error";
                
                var result = new RootDTO { Status = status };
                
                if (doc.RootElement.TryGetProperty("data", out JsonElement dataElement))
                {
                    if (dataElement.ValueKind == JsonValueKind.Object)
                    {
                        var singleOrder = JsonSerializer.Deserialize<OrderDTO>(
                            dataElement.GetRawText(), 
                            _jsonOptions
                        );
                        
                        if (singleOrder != null)
                        {
                            result.Data = new List<OrderDTO> { singleOrder };
                        }
                    }
                    else if (dataElement.ValueKind == JsonValueKind.Array)
                    {
                        result.Data = JsonSerializer
                        .Deserialize<List<OrderDTO>>(dataElement.GetRawText(), _jsonOptions)!;
                    }
                }
                
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error patching order {orderId}: {ex.Message}");
            return null;
        }
    }

    public async Task<RootDTO?> GetOrderAsync(string orderID)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<RootDTO>(
                $"api/order/{orderID}",
                _jsonOptions
            );
            if (response == null || response.Data == null || response.Status != "Success")
            {
                return new RootDTO
                {
                    Status = "Exception",
                    Data = new()
                };
            }
            return response;
        }
        catch
        {
            return new RootDTO
            {
                Status = "Exception",
                Data = new()
            };
        }
    }

    /// <summary>
    /// Deletes an order from the backend by orderId.
    /// </summary>
    /// <param name="orderId">The ID of the order to delete.</param>
    /// <returns>A RootDTO object containing the status and data of the response.</returns>
    public async Task<RootDTO?> DeleteOrderAsync(string orderId)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/order/{orderId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: Could not delete order {orderId}.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = new()
                };
            }
            return new RootDTO
            {
                Status = "Success",
                Data = new()
            };
        }
        catch
        {
            Console.WriteLine($"Error: Could not delete order {orderId}.");
            return new RootDTO
            {
                Status = "Exception",
                Data = new()
            };
        }
    }

    /// <summary>
    /// Posts an order line to the backend.
    /// </summary>s
    public async Task<RootDTO?> PostOrderLineToBackend(OrderDTO orderDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(orderDto, _jsonOptions);
            var response = await _http.PostAsJsonAsync("api/order/", orderDto, _jsonOptions);

            if (response == null)
            {
                Console.WriteLine("Error: Could not post orderline to backend.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = new()
                };
            }
            var result = await response.Content.ReadFromJsonAsync<RootDTO>(_jsonOptions);
            if (result == null)
            {
                Console.WriteLine("Error: Could not post orderline to backend.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = new()
                };
            }
            return result;
        }
        catch
        {
            Console.WriteLine("Error: Could not post orderline to backend.");
            return new RootDTO
            {
                Status = "Exception",
                Data = new()
            };
        }
    }
}


// For converting the JSON property names to kebab-case
public class KebabCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var builder = new StringBuilder();
        
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            
            if (c == '_')
            {
                builder.Append('-');
            }
            else if (i > 0 && char.IsUpper(c))
            {
                builder.Append('-');
                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                builder.Append(char.ToLowerInvariant(c));
            }
        }
        
        return builder.ToString();
    }
}