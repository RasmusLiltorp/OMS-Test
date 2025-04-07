using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DTOs;

namespace OMS_Services;

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

    public async Task<ApiResponse<List<JsonDTO.OrderDataDTO>?>> GetMultipleOrdersAsync(int amount)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<JsonDTO.OrderDataDTO>>>(
                "api/order/?amount=" + amount,
                _jsonOptions
            );

            if (response == null || response.Status != "Success" || response.Data == null)
            {
                return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
                {
                    Status = "Exception",
                    Data = null
                };
            }

            return response;
        }
        catch
        {
            return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
            {
                Status = "Exception",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<List<JsonDTO.OrderDataDTO>?>> GetOrderAsync(string orderID)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<JsonDTO.OrderDataDTO>>>(
                "api/order/" + orderID,
                _jsonOptions
            );

            if (response == null || response.Status != "Success" || response.Data == null)
            {
                return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
                {
                    Status = "Exception",
                    Data = null
                };
            }

            return response;
        }
        catch
        {
            return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
            {
                Status = "Exception",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<List<JsonDTO.OrderDataDTO>?>> PostOrderLineToBackend(JsonDTO.OrderDataDTO orderDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(orderDto, _jsonOptions);
            var response = await _http.PostAsJsonAsync("api/order/", orderDto, _jsonOptions);
            
            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
                {
                    Status = "Exception",
                    Data = null
                };
            }

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<List<JsonDTO.OrderDataDTO>>>(_jsonOptions);

            if (result == null)
            {
                return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
                {
                    Status = "Exception",
                    Data = null
                };
            }

            return result;
        }
        catch
        {
            return new ApiResponse<List<JsonDTO.OrderDataDTO>?>
            {
                Status = "Exception",
                Data = null
            };
        }
    }
}

public class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }
}
