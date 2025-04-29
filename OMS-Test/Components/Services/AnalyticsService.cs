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
                
                if (product == null || 
                    (product.BrandName == null) || 
                    (!string.IsNullOrEmpty(brandName) && product.BrandName != brandName))
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
                        PeriodRevenue = 0,
    
                    };
                    brandResults[currentBrand] = brandData;
                }
                
                brandData.PeriodUnitsSold += lineItem.Amount;
                brandData.PeriodRevenue += lineItem.Price * lineItem.Amount;
            }
        }
        
        return brandResults;    
    }
}