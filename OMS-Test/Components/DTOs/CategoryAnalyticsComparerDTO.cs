namespace DTOs;
public class CategoryAnalyticsComparerDTO
{
    public required string CategoryName { get; set; } = "";
    public required int Period1UnitsSold { get; set; }
    public required decimal Period1Revenue { get; set; }
    public required int Period2UnitsSold { get; set; }
    public required decimal Period2Revenue { get; set; }
}
