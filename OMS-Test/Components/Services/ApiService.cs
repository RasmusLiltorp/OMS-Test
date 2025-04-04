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
    public async Task<ApiResponse<List<OrderLine>?>> GetMultipleOrdersAsync(int amount)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<OrderLine>>>("api/order/?amount=" + amount);

            if (response == null || response.Status != "Success" || response.Data == null)
            {
                return new ApiResponse<List<OrderLine>?>
                {
                    Status = "Exception",
                    Data = null
                };
            }
            return response;
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error fetching orders: {e.Message}");
            return new ApiResponse<List<OrderLine>?>
            {
                Status = "Exception",
                Data = null
            };
        }
    }
}

// Class for holding response - structurized like json-response from backend.
public class ApiResponse<T>
{
    public string Status { get; set; } 
    public T Data { get; set; }
}
