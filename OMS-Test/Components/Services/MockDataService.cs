using System;
using System.Collections.Generic;

namespace OMS_Test.Services
{
    public class MockDataService
    {
        public List<Product> Products { get; private set; }
        public List<OrderLine> OrderLines { get; private set; }

        public MockDataService()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Products = new List<Product>
            {
                new Product { ProductID = "1", ProductName = "Smartphone", Price = 4999.99m, Weight = 0.5m },
                new Product { ProductID = "2", ProductName = "Laptop", Price = 7999.99m, Weight = 2.5m },
                new Product { ProductID = "3", ProductName = "Headphones", Price = 399.99m, Weight = 0.3m },
                new Product { ProductID = "4", ProductName = "Tablet", Price = 2999.99m, Weight = 0.8m }
            };

            OrderLines = new List<OrderLine>
            {
                new OrderLine { Customer = "John Doe", ProductIDs = new List<string> { "1", "2" }, OrderId = 1, OrderDate = new DateOnly(2025, 3, 1) },
                new OrderLine { Customer = "Jane Doe", ProductIDs = new List<string> { "3", "4" }, OrderId = 2, OrderDate = new DateOnly(2025, 3, 5) },
                new OrderLine { Customer = "James Bond", ProductIDs = new List<string> { "1", "2" }, OrderId = 3, OrderDate = new DateOnly(2025, 3, 10) },
                new OrderLine { Customer = "Jason Bourne", ProductIDs = new List<string> { "3", "4" }, OrderId = 4, OrderDate = new DateOnly(2025, 3, 15) },
                new OrderLine { Customer = "Fætter Guf", ProductIDs = new List<string> { "1", "2" }, OrderId = 5, OrderDate = new DateOnly(2025, 3, 20) },
                new OrderLine { Customer = "Casper Holm Bach", ProductIDs = new List<string> { "3", "4", "2", "1" }, OrderId = 6, OrderDate = new DateOnly(2025, 3, 25) },
                new OrderLine { Customer = "Tobias Hansen", ProductIDs = new List<string> { "1", "2" }, OrderId = 7, OrderDate = new DateOnly(2025, 3, 30) },
                new OrderLine { Customer = "Karem Jahjah", ProductIDs = new List<string> { "3", "4" }, OrderId = 8, OrderDate = new DateOnly(2025, 3, 28) },
                new OrderLine { Customer = "Lucas Barlach", ProductIDs = new List<string> { "1", "2" }, OrderId = 9, OrderDate = new DateOnly(2025, 3, 26) },
                new OrderLine { Customer = "Mads Mikkelsen", ProductIDs = new List<string> { "3", "4" }, OrderId = 10, OrderDate = new DateOnly(2025, 3, 24) }
            };
        }

        public List<Product> GetProductsForOrder(OrderLine order)
        {
            return Products.Where(p => order.ProductIDs.Contains(p.ProductID)).ToList();
        }

        public decimal CalculateOrderTotal(OrderLine order)
        {
            return GetProductsForOrder(order).Sum(p => p.Price);
        }
        public decimal CalculateOrderWeight(OrderLine order)
        {
            return GetProductsForOrder(order).Sum(p => p.Weight);
        }
    }

    public class Product
    {
        public required string ProductID { get; set; }
        public required string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
    }

    public class OrderLine
    {
        public required string Customer { get; set; }
        public required List<string> ProductIDs { get; set; }
        public required int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
    }
}