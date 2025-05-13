using System.Text.Json.Serialization;

public class StockStatusResponseDTO
{
    public required string Status { get; set; }
    public int Data { get; set; }
}

public class StockAmountResponseDTO
{
    public required int Amount { get; set; }
}
