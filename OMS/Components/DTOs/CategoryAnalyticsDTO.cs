namespace DTOs
{
    
public class CategoryAnalyticsDTO
{
    public required string CategoryName { get; set; }
    public required int TotalUnitsSold { get; set; }
    public required decimal TotalRevenue { get; set; }
}
}