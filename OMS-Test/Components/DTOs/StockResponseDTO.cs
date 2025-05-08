using System.Text.Json.Serialization;

public class StockStatusResponseDTO
{
    public string Status { get; set; }
    public int Data { get; set; }
}

public class StockAmountResponseDTO
{
    public int Amount { get; set; }
}
