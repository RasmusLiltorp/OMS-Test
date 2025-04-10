using System;
using System.Collections.Generic;
using System.Linq;
using DTOs;

namespace OMS_Test.Services
{
    public class MockDataService
    {
        public List<ProductDTO> Products { get; private set; } = new List<ProductDTO>();
        public List<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();

        public MockDataService()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Products = new List<ProductDTO>
            {
                new ProductDTO { ProductID = "1", ProductName = "Smartphone", BrandName = "Apple", ProductCategory = "Electronic", Price = 4999.99m, Weight = 0.5m },
                new ProductDTO { ProductID = "2", ProductName = "Laptop", BrandName = "Apple", ProductCategory = "Electronic", Price = 7999.99m, Weight = 2.5m },
                new ProductDTO { ProductID = "3", ProductName = "Headphones", BrandName = "Sony", ProductCategory = "Accessories", Price = 399.99m, Weight = 0.3m },
                new ProductDTO { ProductID = "4", ProductName = "Tablet", BrandName = "Samsung", ProductCategory = "Electronic", Price = 2999.99m, Weight = 0.8m },
                new ProductDTO { ProductID = "5", ProductName = "Vingummibamser", BrandName = "Haribo", ProductCategory = "Confectionery", Price = 19.99m, Weight = 0.2m }
            };

            OrderLines = new List<OrderLine>
            {
                new OrderLine {
                    Customer = "John Doe",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "1", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 2 }
                    },
                    OrderId = 1,
                    OrderDate = new DateOnly(2025, 3, 1),
                    Email = "john.doe@example.com",
                    TrackAndTrace = "123456789"
                },
                new OrderLine {
                    Customer = "Jane Doe",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "3", Quantity = 1 },
                        new OrderProduct { ProductID = "4", Quantity = 1 }
                    },
                    OrderId = 2,
                    OrderDate = new DateOnly(2025, 3, 5),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "James Bond",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "1", Quantity = 2 },
                        new OrderProduct { ProductID = "2", Quantity = 1 }
                    },
                    OrderId = 3,
                    OrderDate = new DateOnly(2025, 3, 10),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Jason Bourne",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "3", Quantity = 3 },
                        new OrderProduct { ProductID = "4", Quantity = 1 }
                    },
                    OrderId = 4,
                    OrderDate = new DateOnly(2025, 3, 15),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Fætter Guf",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "1", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 1 },
                        new OrderProduct { ProductID = "3", Quantity = 2 },
                        new OrderProduct { ProductID = "5", Quantity = 25 }
                    },
                    OrderId = 5,
                    OrderDate = new DateOnly(2025, 3, 20),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Casper Holm Bach",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "3", Quantity = 2 },
                        new OrderProduct { ProductID = "4", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 1 },
                        new OrderProduct { ProductID = "1", Quantity = 3 }
                    },
                    OrderId = 6,
                    OrderDate = new DateOnly(2025, 3, 25),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Tobias Hansen",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "1", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 2 }
                    },
                    OrderId = 7,
                    OrderDate = new DateOnly(2025, 3, 30),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Karem Jahjah",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "3", Quantity = 2 },
                        new OrderProduct { ProductID = "4", Quantity = 2 }
                    },
                    OrderId = 8,
                    OrderDate = new DateOnly(2025, 3, 28),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Lucas Barlach",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "1", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 1 }
                    },
                    OrderId = 9,
                    OrderDate = new DateOnly(2025, 3, 26),
                    Email = "",
                    TrackAndTrace = ""
                },
                new OrderLine {
                    Customer = "Mads Mikkelsen",
                    Products = new List<OrderProduct> {
                        new OrderProduct { ProductID = "3", Quantity = 2 },
                        new OrderProduct { ProductID = "4", Quantity = 1 }
                    },
                    OrderId = 10,
                    OrderDate = new DateOnly(2025, 3, 24),
                    Email = "",
                    TrackAndTrace = ""
                }
            };

            // Calculate prices for order products that don't have it set (for whatever reason)
            foreach (var order in OrderLines)
            {
                foreach (var orderProduct in order.Products)
                {
                    if (orderProduct.Price == 0)
                    {
                        var product = Products.FirstOrDefault(p => p.ProductID == orderProduct.ProductID);
                        if (product != null)
                        {
                            orderProduct.Price = product.Price;
                        }
                    }
                }
            }
        }

        public List<ProductDTO> GetProductsForOrder(OrderLine order)
        {
            return Products.Where(p => order.Products.Any(op => op.ProductID == p.ProductID)).ToList();
        }

        public decimal CalculateOrderTotal(OrderLine order)
        {
            decimal total = 0;
            foreach (var orderProduct in order.Products)
            {
                var product = Products.FirstOrDefault(p => p.ProductID == orderProduct.ProductID);
                if (product != null)
                {
                    total += orderProduct.Price * orderProduct.Quantity;
                }
            }
            return total;
        }

        public List<BrandAnalyticsDTO> GetBrandAnalytics(DateOnly fromDate, DateOnly toDate)
        {
            var filteredOrders = OrderLines
                .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                .ToList();

            return Products
                .GroupBy(p => p.BrandName)
                .Select(group => new BrandAnalyticsDTO
                {
                    BrandName = group.Key,
                    TotalUnitsSold = filteredOrders.SelectMany(o => o.Products)
                        .Where(op => group.Select(g => g.ProductID).Contains(op.ProductID))
                        .Sum(op => op.Quantity),
                    TotalRevenue = filteredOrders.SelectMany(o => o.Products)
                        .Where(op => group.Select(g => g.ProductID).Contains(op.ProductID))
                        .Sum(op => op.Quantity * Products.First(p => p.ProductID == op.ProductID).Price)
                }).ToList();
        }



        public List<CategoryAnalyticsDTO> GetCategoryAnalytics(DateOnly fromDate, DateOnly toDate)
        {
            var filteredOrders = OrderLines
                .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                .ToList();

            return Products
                .GroupBy(p => p.ProductCategory)
                .Select(group => new CategoryAnalyticsDTO
                {
                    CategoryName = group.Key,
                    TotalUnitsSold = filteredOrders.SelectMany(o => o.Products)
                        .Where(op => group.Select(g => g.ProductID).Contains(op.ProductID))
                        .Sum(op => op.Quantity),
                    TotalRevenue = filteredOrders.SelectMany(o => o.Products)
                        .Where(op => group.Select(g => g.ProductID).Contains(op.ProductID))
                        .Sum(op => op.Quantity * Products.First(p => p.ProductID == op.ProductID).Price)
                }).ToList();
        }

        public List<string> GetAllBrands()
        {
            return Products.Select(p => p.BrandName).Distinct().ToList();
        }

        public List<string> GetAllCategories()
        {
            return Products.Select(p => p.ProductCategory).Distinct().ToList();
        }

        public List<(DateOnly Date, int UnitsSold)> GetDailyBrandUnitsSold(string brandName, DateOnly fromDate, DateOnly toDate)
        {
            var productIds = Products.Where(p => p.BrandName == brandName).Select(p => p.ProductID).ToList();
            var dailyData = new List<(DateOnly, int)>();

            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                int unitsSold = OrderLines
                    .Where(o => o.OrderDate == date)
                    .SelectMany(o => o.Products)
                    .Where(op => productIds.Contains(op.ProductID))
                    .Sum(op => op.Quantity);

                dailyData.Add((date, unitsSold));
            }

            return dailyData;
        }

        public List<(DateOnly Date, int UnitsSold)> GetDailyCategoryUnitsSold(string categoryName, DateOnly fromDate, DateOnly toDate)
        {
            var productIds = Products.Where(p => p.ProductCategory == categoryName).Select(p => p.ProductID).ToList();
            var dailyData = new List<(DateOnly, int)>();

            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                int unitsSold = OrderLines
                    .Where(o => o.OrderDate == date)
                    .SelectMany(o => o.Products)
                    .Where(op => productIds.Contains(op.ProductID))
                    .Sum(op => op.Quantity);

                dailyData.Add((date, unitsSold));
            }

            return dailyData;
        }


    }
}
