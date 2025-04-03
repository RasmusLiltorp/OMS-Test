using System.Collections;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiService"/> class.
    /// </summary>
    /// <param name="http">The HTTP client used for making requests.</param>

    public ApiService(HttpClient http)
    {
        _http = http;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, 
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull};            
    }

    // Get multiple orders from backend. Get X amount
    public async Task<List<OrderLine>?> GetMultipleOrdersAsync(int amount)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<OrderLine>>($"api/order/?amount={amount}");
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error fetching orders: {e.Message}");
            return new List<OrderLine>();
        }
    }
}

// Class for holding response - structurized like json-response from backend.
public class ApiResponse<T>
{
    public string Status { get; set; } 
    public T Data { get; set; }
}
