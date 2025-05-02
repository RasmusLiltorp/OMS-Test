using DTOs;

namespace OMS_Services;
public class AnalyticsService
{
    private readonly DataService _dataService;
    public AnalyticsService(DataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Returns Dictionary with BrandAnalyticsDTO for a given brand in the system.
    /// </summary>
    public Dictionary<string, BrandAnalyticsDTO> CompareBrandOverall(string brandName)
    {
        Dictionary<string, BrandAnalyticsDTO> brandResults = new();
        
        foreach (var order in _dataService.OrderLines)
        {
            foreach (var lineItem in order.LineElements)
            {
                var product = _dataService.Products.FirstOrDefault(p => p.ProductID == lineItem.ProductUuid);
                
                if (product == null || 
                    product.BrandName == null || 
                    (!string.IsNullOrEmpty(brandName) && product.BrandName != brandName))
                {
                    continue;
                }
                
                string currentBrand = product.BrandName;
                
                if (!brandResults.TryGetValue(currentBrand, out BrandAnalyticsDTO? brandData))
                {
                    brandData = new BrandAnalyticsDTO
                    {
                        BrandName = currentBrand,
                        TotalUnitsSold = 0,
                        TotalRevenue = 0
                    };
                    brandResults[currentBrand] = brandData;
                }
                
                brandData.TotalUnitsSold += lineItem.Amount;
                brandData.TotalRevenue += lineItem.Price * lineItem.Amount;
            }
        }
        
        return brandResults;
    }

    /// <summary>
    /// This class is responsible for analyzing and comparing brands based on order data.
    /// It should find all the products, with the given brand name (retrieved from DataService using GetAllBrands())
    /// and return when and how many sales the brand has had in the given time period.
    /// </summary>
    public Dictionary<string, BrandComparisonDTO> CompareBrandsInterval(string brandName, DateOnly from, DateOnly to)
    {
        Dictionary<string, BrandComparisonDTO> brandResults = new();
        
        var fromDate = from.ToDateTime(TimeOnly.MinValue);
        var toDate = to.ToDateTime(TimeOnly.MaxValue);
        
        var ordersInRange = _dataService.OrderLines
            .Where(order => order.Date >= fromDate && order.Date <= toDate)
            .ToList();
        
        foreach (var order in ordersInRange)
        {
            foreach (var lineItem in order.LineElements)
            {
                var product = _dataService.Products.FirstOrDefault(p => p.ProductID == lineItem.ProductUuid);
                
                if (product == null || product.BrandName == null)
                {
                    continue;
                }
                
                if (!string.IsNullOrEmpty(brandName) && product.BrandName != brandName)
                {
                    continue;
                }
                
                string currentBrand = product.BrandName;
                
                if (!brandResults.TryGetValue(currentBrand, out BrandComparisonDTO? brandData))
                {
                    brandData = new BrandComparisonDTO
                    {
                        BrandName = currentBrand,
                        PeriodUnitsSold = 0,
                        PeriodRevenue = 0
                    };
                    brandResults[currentBrand] = brandData;
                }
                
                brandData.PeriodUnitsSold += lineItem.Amount;
                brandData.PeriodRevenue += lineItem.Price * lineItem.Amount;
            }
        }
        
        return brandResults;    
    }
    
    /// <summary>
    /// This method analyzes and compares product categories based on order data.
    /// It finds all products with the given category name and returns sales data for the specified time period.
    /// </summary>
    public Dictionary<string, CategoryPeriodDTO> CompareCategoriesInterval(string categoryName, DateOnly from, DateOnly to)
    {
        Dictionary<string, CategoryPeriodDTO> categoryResults = new();
        
        var fromDate = from.ToDateTime(TimeOnly.MinValue);
        var toDate = to.ToDateTime(TimeOnly.MaxValue);
        
        var ordersInRange = _dataService.OrderLines
            .Where(order => order.Date >= fromDate && order.Date <= toDate)
            .ToList();

        
        foreach (var order in ordersInRange)
        {
            foreach (var lineItem in order.LineElements)
            {
                var product = _dataService.Products.FirstOrDefault(p => p.ProductID == lineItem.ProductUuid);
                
                if (product == null || product.ProductCategory == null)
                {
                    continue;
                }
                
                if (!string.IsNullOrEmpty(categoryName) && product.ProductCategory != categoryName)
                {
                    continue;
                }
                
                string currentCategory = product.ProductCategory;
                
                if (!categoryResults.TryGetValue(currentCategory, out CategoryPeriodDTO? categoryData))
                {
                    categoryData = new CategoryPeriodDTO
                    {
                        CategoryName = currentCategory,
                        PeriodUnitsSold = 0,
                        PeriodRevenue = 0
                    };
                    categoryResults[currentCategory] = categoryData;
                }
                
                categoryData.PeriodUnitsSold += lineItem.Amount;
                categoryData.PeriodRevenue += lineItem.Price * lineItem.Amount;
            }
        }
        
        return categoryResults;    
    }

    /// <summary>
    /// Gets product sales data aggregated by day for a specific product within a date range.
    /// </summary>
    public List<ProductSalesDataDTO> GetProductSalesData(string productId, DateTime startDate, DateTime endDate)
    {
        var result = new List<ProductSalesDataDTO>();
        
        var product = string.IsNullOrEmpty(productId) ? null : 
                      _dataService.Products.FirstOrDefault(p => p.ProductID == productId);
        
        var ordersInRange = _dataService.OrderLines
            .Where(order => order.Date >= startDate && order.Date <= endDate)
            .ToList();
            
        var dailyData = new Dictionary<DateTime, ProductSalesDataDTO>();
        
        foreach (var order in ordersInRange)
        {
            var orderDate = order.Date.Date;
            
            foreach (var lineItem in order.LineElements)
            {
                if (!string.IsNullOrEmpty(productId) && lineItem.ProductUuid != productId)
                    continue;
                    
                var lineItemProduct = _dataService.Products.FirstOrDefault(p => p.ProductID == lineItem.ProductUuid);
                if (lineItemProduct == null)
                    continue;
                
                if (!dailyData.TryGetValue(orderDate, out var dataPoint))
                {
                    dataPoint = new ProductSalesDataDTO
                    {
                        ProductId = lineItem.ProductUuid,
                        ProductName = lineItemProduct.ProductName,
                        Date = orderDate,
                        UnitsSold = 0,
                        Revenue = 0
                    };
                    dailyData[orderDate] = dataPoint;
                }
                
                dataPoint.UnitsSold += lineItem.Amount;
                dataPoint.Revenue += lineItem.Price * lineItem.Amount;
            }
        }
        
        result = dailyData.Values.OrderBy(d => d.Date).ToList();
        
        return result;
    }
    
    /// <summary>
    /// Creates a ProductSalesChartDTO with formatted data for the chart.
    /// Aggregates data by day, week, or month depending on the timeframe and date range.
    /// </summary>
    public ProductSalesChartDTO GetProductChartData(string productId, DateTime startDate, DateTime endDate, string rangeType = "week")
    {
        var rawData = GetProductSalesData(productId, startDate, endDate);
        var chartDto = new ProductSalesChartDTO();
        
        if (!rawData.Any())
            return chartDto;
            
        var productName = rawData.FirstOrDefault()?.ProductName ?? "Unknown Product";
        
        TimeSpan dateRange = endDate - startDate;
        
        if (dateRange.TotalDays <= 14)
        {
            foreach (var dataPoint in rawData.OrderBy(d => d.Date))
            {
                chartDto.Labels.Add($"{dataPoint.Date:MMM dd}");
                chartDto.UnitsSoldData.Add(dataPoint.UnitsSold);
                chartDto.RevenueData.Add(dataPoint.Revenue);
            }
        }
        else if (rangeType.ToLower() == "week")
        {
            var groupedData = rawData.GroupBy(d => 
                new { Year = d.Date.Year, Week = System.Globalization.ISOWeek.GetWeekOfYear(d.Date) });
                
            foreach (var group in groupedData.OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Week))
            {
                var firstDayOfWeek = group.Min(d => d.Date);
                
                chartDto.Labels.Add($"Week {group.Key.Week} ({firstDayOfWeek:MMM dd})");
                chartDto.UnitsSoldData.Add(group.Sum(d => d.UnitsSold));
                chartDto.RevenueData.Add(group.Sum(d => d.Revenue));
            }
        }
        else 
        {
            var groupedData = rawData.GroupBy(d => new { d.Date.Year, d.Date.Month });
            
            foreach (var group in groupedData.OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month))
            {
                chartDto.Labels.Add($"{new DateTime(group.Key.Year, group.Key.Month, 1):MMM yyyy}");
                chartDto.UnitsSoldData.Add(group.Sum(d => d.UnitsSold));
                chartDto.RevenueData.Add(group.Sum(d => d.Revenue));
            }
        }
        
        return chartDto;
    }
    
    /// <summary>
    /// Calculates next and previous time periods based on a current range and range type.
    /// </summary>
    public ProductTimeRangeDTO GetAdjacentTimeRange(DateTime startDate, DateTime endDate, string rangeType, bool isNext)
    {
        var result = new ProductTimeRangeDTO
        {
            RangeType = rangeType
        };
        
        var days = (endDate - startDate).Days;
        
        if (rangeType.ToLower() == "week")
        {
            if (isNext)
            {
                result.StartDate = startDate.AddDays(7);
                result.EndDate = endDate.AddDays(7);
            }
            else
            {
                result.StartDate = startDate.AddDays(-7);
                result.EndDate = endDate.AddDays(-7);
            }
        }
        else 
        {
            if (isNext)
            {
                result.StartDate = startDate.AddMonths(1);
                result.EndDate = endDate.AddMonths(1);
            }
            else
            {
                result.StartDate = startDate.AddMonths(-1);
                result.EndDate = endDate.AddMonths(-1);
            }
        }
        
        return result;
    }
}