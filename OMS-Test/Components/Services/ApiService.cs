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
                        result.Data = JsonSerializer.Deserialize<List<OrderDTO>>(
                            dataElement.GetRawText(),
                            _jsonOptions
                        ) ?? new List<OrderDTO>();
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

    public async Task<RootDTO?> GetOrderAsync(string orderId)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<RootDTO>(
                $"api/order/{orderId}",
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

    public async Task<RootDTO?> PostOrderLineToBackend(OrderDTO orderDto)
    {
        try
        {
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

    public async Task<List<BrandAnalyticsDTO>?> GetBrandAnalyticsAsync(DateOnly from, DateOnly to)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<BrandAnalyticsDTO>>(
                $"api/analytics/brands?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}",
                _jsonOptions
            );
            return response;
        }
        catch
        {
            Console.WriteLine("Error fetching brand analytics.");
            return null;
        }
    }

    public async Task<List<CategoryAnalyticsDTO>?> GetCategoryAnalyticsAsync(DateOnly from, DateOnly to)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<CategoryAnalyticsDTO>>(
                $"api/analytics/categories?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}",
                _jsonOptions
            );
            return response;
        }
        catch
        {
            Console.WriteLine("Error fetching category analytics.");
            return null;
        }
    }
}

/// <summary>
/// Converts C# property names to kebab-case (e.g., "totalCost" -> "total-cost").
/// </summary>
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
