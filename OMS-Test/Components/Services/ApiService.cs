using System.Net.Http;
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
            Console.WriteLine(nameof(GetMultipleOrdersAsync));
            var response = await _http.GetFromJsonAsync<RootDTO>(
                $"api/order/?amount={amount}",
                _jsonOptions
            );
            Console.WriteLine("Post Json Deserialize " + nameof(GetMultipleOrdersAsync));
            if (response == null || response.Data == null || response.Status != "Success")
            {
                Console.WriteLine("Error: No orders found or error occurred while fetching orders.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = null
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
                Data = null
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
            var json = JsonSerializer.Serialize(patchData, _jsonOptions);

            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/order/{orderId}")
            {
            };

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new RootDTO
                {
                    Status = "Exception",
                    Data = null
                };
            }
            var result = await response.Content.ReadFromJsonAsync<RootDTO>(_jsonOptions);
            return result;
        }
        catch
        {
                Console.WriteLine($"Error: Could not patch order {orderId}.");
                return new RootDTO { Status = "Exception", Data = null };
        }

    }

    public async Task<RootDTO?> GetOrderAsync(string orderID)
    {
        try
        {
            Console.WriteLine(nameof(GetOrderAsync));
            var response = await _http.GetFromJsonAsync<RootDTO>(
                $"api/order/{orderID}",
                _jsonOptions
            );
            Console.WriteLine("Post Json Deserialize " + nameof(GetOrderAsync));
            if (response == null || response.Data == null || response.Status != "Success")
            {
                return new RootDTO
                {
                    Status = "Exception",
                    Data = null
                };
            }
            return response;
        }
        catch
        {
            return new RootDTO
            {
                Status = "Exception",
                Data = null
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
                    Data = null
                };
            }
            return new RootDTO
            {
                Status = "Success",
                Data = null
            };
        }
        catch
        {
            Console.WriteLine($"Error: Could not delete order {orderId}.");
            return new RootDTO
            {
                Status = "Exception",
                Data = null
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
            Console.WriteLine($"Sending order: {json}");

            if (response == null)
            {
                Console.WriteLine("Error: Could not post orderline to backend.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = null
                };
            }
            Console.WriteLine($"Response status: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

            var result = await response.Content.ReadFromJsonAsync<RootDTO>(_jsonOptions);
            if (result == null)
            {
                Console.WriteLine("Error: Could not post orderline to backend.");
                return new RootDTO
                {
                    Status = "Exception",
                    Data = null
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
                Data = null
            };
        }
    }
}
