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

    public ApiService(HttpClient http)
    {
        _http = http;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    // GET all orders
    public async Task<OrderLine[]> GetAllOrdersAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<OrderLine[]>>("api/order");
        }
        catch(Exception e)
        {
            Console.Writeline($"Error fetching orders: {e.Message}");
            return new List<OrderLine>();
        }
    }

    // GET order by ID
    public async Task<OrderLine> GetOrderByIdAsync(int orderId)
    {
        try
        {
            return await _http.GetFromJsonAsync<OrderLine>($"api/order/{orderId}");
        }
        catch(Exception e)
        {
            Console.Writeline($"Error fetching {orderId}: {e.Message}");
            return null;
        }    
    }

    // Send data (POST)

    // UPDATE data (PUT)

    // DELETE data (DELETE)
}
