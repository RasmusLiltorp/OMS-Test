namespace DTOs;
public class CategoryAnalyticsComparerDTO
{
    public string CategoryName { get; set; } = "";
    public int Period1UnitsSold { get; set; }
    public decimal Period1Revenue { get; set; }
    public int Period2UnitsSold { get; set; }
    public decimal Period2Revenue { get; set; }
}
