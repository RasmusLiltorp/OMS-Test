using System;
using System.Collections.Generic;
using System.Linq;
using DTOs;

namespace OMS_Services
{
    /// <summary>
    /// Provides mock data for testing purposes
    /// </summary>
    public class MockDataService
    {
        public List<ProductDTO> Products { get; private set; } = new();
        public List<OrderDTO> Orders { get; private set; } = new();

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

            Orders = new List<OrderDTO>
            {
                new OrderDTO 
                {
                    OrderId = "1",
                    Date = new DateTime(2025, 3, 1),
                    FulfillmentState = 1,
                    TotalCost = 4999.99m + (7999.99m * 2),
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "1",
                        Name = "John Doe"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "123 Main St",
                        Address2 = "Apt 101",
                        ZipcodeId = 5000,
                        CountryId = 45,
                        TrackingNumber = "123456789"
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 1, Amount = 1, Price = 4999.99m },
                        new LineElementDTO { ProductUuid = 2, Amount = 2, Price = 7999.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "2",
                    Date = new DateTime(2025, 3, 5),
                    FulfillmentState = 1,
                    TotalCost = 399.99m + 2999.99m,
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "2",
                        Name = "Jane Doe"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "456 Oak Ave",
                        Address2 = "",
                        ZipcodeId = 5001,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 3, Amount = 1, Price = 399.99m },
                        new LineElementDTO { ProductUuid = 4, Amount = 1, Price = 2999.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "3",
                    Date = new DateTime(2025, 3, 10),
                    FulfillmentState = 1,
                    TotalCost = (4999.99m * 2) + 7999.99m,
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "7",
                        Name = "James Bond"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "007 Secret St",
                        Address2 = "",
                        ZipcodeId = 5007,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 1, Amount = 2, Price = 4999.99m },
                        new LineElementDTO { ProductUuid = 2, Amount = 1, Price = 7999.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "4",
                    Date = new DateTime(2025, 3, 15),
                    FulfillmentState = 1,
                    TotalCost = (399.99m * 3) + 2999.99m,
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "4",
                        Name = "Jason Bourne"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "Unknown Location",
                        Address2 = "",
                        ZipcodeId = 9999,
                        CountryId = 1
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 3, Amount = 3, Price = 399.99m },
                        new LineElementDTO { ProductUuid = 4, Amount = 1, Price = 2999.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "5",
                    Date = new DateTime(2025, 3, 20),
                    FulfillmentState = 1,
                    TotalCost = 4999.99m + 7999.99m + (399.99m * 2) + (19.99m * 25),
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "5",
                        Name = "Fætter Guf"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "Candy Lane 5",
                        Address2 = "",
                        ZipcodeId = 5005,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 1, Amount = 1, Price = 4999.99m },
                        new LineElementDTO { ProductUuid = 2, Amount = 1, Price = 7999.99m },
                        new LineElementDTO { ProductUuid = 3, Amount = 2, Price = 399.99m },
                        new LineElementDTO { ProductUuid = 5, Amount = 25, Price = 19.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "6",
                    Date = new DateTime(2025, 3, 25),
                    FulfillmentState = 1,
                    TotalCost = (399.99m * 2) + 2999.99m + 7999.99m + (4999.99m * 3),
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "6",
                        Name = "Casper Holm Bach"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "Tech Avenue 42",
                        Address2 = "Floor 3",
                        ZipcodeId = 5006,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 3, Amount = 2, Price = 399.99m },
                        new LineElementDTO { ProductUuid = 4, Amount = 1, Price = 2999.99m },
                        new LineElementDTO { ProductUuid = 2, Amount = 1, Price = 7999.99m },
                        new LineElementDTO { ProductUuid = 1, Amount = 3, Price = 4999.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "7",
                    Date = new DateTime(2025, 3, 30),
                    FulfillmentState = 1,
                    TotalCost = 4999.99m + (7999.99m * 2),
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "7",
                        Name = "Tobias Hansen"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "Developer Street 10",
                        Address2 = "",
                        ZipcodeId = 5100,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 1, Amount = 1, Price = 4999.99m },
                        new LineElementDTO { ProductUuid = 2, Amount = 2, Price = 7999.99m }
                    }
                },
                
                new OrderDTO 
                {
                    OrderId = "8",
                    Date = new DateTime(2025, 3, 28),
                    FulfillmentState = 1,
                    TotalCost = (399.99m * 2) + (2999.99m * 2),
                    CustomerInfo = new CustomerInfoDTO 
                    {
                        CustomerId = "8",
                        Name = "Karem Jahjah"
                    },
                    ShippingInfo = new ShippingInfoDTO 
                    {
                        Address1 = "Creative Lane 8",
                        Address2 = "",
                        ZipcodeId = 5200,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO> 
                    {
                        new LineElementDTO { ProductUuid = 3, Amount = 2, Price = 399.99m },
                        new LineElementDTO { ProductUuid = 4, Amount = 2, Price = 2999.99m }
                    }
                },

                new OrderDTO
                {
                    OrderId = "9",
                    Date = new DateTime(2025, 3, 30),
                    FulfillmentState = 1,
                    TotalCost = (4999.99m * 2) + (7999.99m * 2),
                    CustomerInfo = new CustomerInfoDTO
                    {
                        CustomerId = "9",
                        Name = "Lucas Barlach"
                    },
                    ShippingInfo = new ShippingInfoDTO
                    {
                        Address1 = "Test Street 1",
                        Address2 = "",
                        ZipcodeId = 5300,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO>
                    {
                        new LineElementDTO { ProductUuid = 1, Amount = 2, Price = 4999.99m },
                        new LineElementDTO { ProductUuid = 2, Amount = 2, Price = 7999.99m }
                    }
                },

                new OrderDTO
                {
                    OrderId = "10",
                    Date = new DateTime(2025, 3, 31),
                    FulfillmentState = 1,
                    TotalCost = (399.99m * 3) + (2999.99m * 2),
                    CustomerInfo = new CustomerInfoDTO
                    {
                        CustomerId = "10",
                        Name = "Mikkel Møller"
                    },
                    ShippingInfo = new ShippingInfoDTO
                    {
                        Address1 = "Mock Street 10",
                        Address2 = "",
                        ZipcodeId = 5400,
                        CountryId = 45
                    },
                    LineElements = new List<LineElementDTO>
                    {
                        new LineElementDTO { ProductUuid = 3, Amount = 3, Price = 399.99m },
                        new LineElementDTO { ProductUuid = 4, Amount = 2, Price = 2999.99m }
                    }
                }
            };
        }

        /// <summary>
        /// Gets the list of products for a specific order
        /// </summary>
        public List<ProductDTO> GetProductsForOrder(OrderDTO order)
        {
            List<ProductDTO> orderProducts = new List<ProductDTO>();
            foreach (var lineElement in order.LineElements)
            {
                var productId = lineElement.ProductUuid.ToString();
                var product = Products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null)
                {
                    orderProducts.Add(product);
                }
            }
            return orderProducts;
        }

        /// <summary>
        /// Calculates the total price of an order
        /// </summary>
        public decimal CalculateOrderTotal(OrderDTO order)
        {
            decimal total = 0;
            foreach (var lineElement in order.LineElements)
            {
                total += lineElement.Price * lineElement.Amount;
            }
            return total;
        }

        /// <summary>
        /// Calculates the total weight of an order
        /// </summary>
        public decimal CalculateOrderWeight(OrderDTO order)
        {
            decimal weight = 0;
            foreach (var lineElement in order.LineElements)
            {
                var productId = lineElement.ProductUuid.ToString();
                var product = Products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null)
                {
                    weight += product.Weight * lineElement.Amount;
                }
            }
            return weight;
        }
    }
}