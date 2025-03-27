using System;
using System.Collections.Generic;
using DTOs;

namespace OMS_Test.Services
{
    public class MockDataService
    {
        public List<ProductDTO> Products { get; private set; } = new();
        public List<OrderLine> OrderLines { get; private set; } = new();

        public MockDataService()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            Products = new List<ProductDTO>
            {
                new ProductDTO { ProductID = "1", ProductName = "Smartphone", Price = 4999.99m, Weight = 0.5m },
                new ProductDTO { ProductID = "2", ProductName = "Laptop", Price = 7999.99m, Weight = 2.5m },
                new ProductDTO { ProductID = "3", ProductName = "Headphones", Price = 399.99m, Weight = 0.3m },
                new ProductDTO { ProductID = "4", ProductName = "Tablet", Price = 2999.99m, Weight = 0.8m },
                new ProductDTO { ProductID = "5", ProductName = "Haribo Vingummibamser", Price = 19.99m, Weight = 0.2m },
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
                    OrderDate = new DateTime(2025, 3, 1),
                    TrackAndTrace = "123456789"
                },
                new OrderLine { 
                    Customer = "Jane Doe", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "3", Quantity = 1 },
                        new OrderProduct { ProductID = "4", Quantity = 1 }
                    }, 
                    OrderId = 2, 
                    OrderDate = new DateTime(2025, 3, 5),
                    TrackAndTrace = ""
                },
                new OrderLine { 
                    Customer = "James Bond", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "1", Quantity = 2 },
                        new OrderProduct { ProductID = "2", Quantity = 1 }
                    }, 
                    OrderId = 3, 
                    OrderDate = new DateTime(2025, 3, 10),
                    TrackAndTrace = ""
                },
                new OrderLine { 
                    Customer = "Jason Bourne", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "3", Quantity = 3 },
                        new OrderProduct { ProductID = "4", Quantity = 1 }
                    }, 
                    OrderId = 4, 
                    OrderDate = new DateTime(2025, 3, 15),
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
                    OrderDate = new DateTime(2025, 3, 20),
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
                    OrderDate = new DateTime(2025, 3, 25),
                    TrackAndTrace = ""
                },
                new OrderLine { 
                    Customer = "Tobias Hansen", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "1", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 2 }
                    }, 
                    OrderId = 7, 
                    OrderDate = new DateTime(2025, 3, 30),
                    TrackAndTrace = ""
                },
                new OrderLine { 
                    Customer = "Karem Jahjah", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "3", Quantity = 2 },
                        new OrderProduct { ProductID = "4", Quantity = 2 }
                    }, 
                    OrderId = 8, 
                    OrderDate = new DateTime(2025, 3, 28),
                    TrackAndTrace = ""
                },
                new OrderLine { 
                    Customer = "Lucas Barlach", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "1", Quantity = 1 },
                        new OrderProduct { ProductID = "2", Quantity = 1 }
                    }, 
                    OrderId = 9, 
                    OrderDate = new DateTime(2025, 3, 26),
                    TrackAndTrace = ""
                },
                new OrderLine { 
                    Customer = "Mads Mikkelsen", 
                    Products = new List<OrderProduct> { 
                        new OrderProduct { ProductID = "3", Quantity = 2 },
                        new OrderProduct { ProductID = "4", Quantity = 1 }
                    }, 
                    OrderId = 10, 
                    OrderDate = new DateTime(2025, 3, 24),
                    TrackAndTrace = ""
                }
            };

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
        
        public decimal CalculateOrderWeight(OrderLine order)
        {
            decimal weight = 0;
            foreach (var orderProduct in order.Products)
            {
                var product = Products.FirstOrDefault(p => p.ProductID == orderProduct.ProductID);
                if (product != null)
                {
                    weight += product.Weight * orderProduct.Quantity;
                }
            }
            return weight;
        }
    }
}