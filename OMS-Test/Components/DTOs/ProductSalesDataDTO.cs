namespace DTOs;

public class ProductSalesDataDTO
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int UnitsSold { get; set; }
    public decimal Revenue { get; set; }
}

public class ProductSalesChartDTO
{
    public List<string> Labels { get; set; } = new List<string>();
    public List<int> UnitsSoldData { get; set; } = new List<int>();
    public List<decimal> RevenueData { get; set; } = new List<decimal>();
    public List<DateTime> DateLabels { get; set; } = new List<DateTime>();
}

public class ProductTimeRangeDTO
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string RangeType { get; set; } = "week";
}