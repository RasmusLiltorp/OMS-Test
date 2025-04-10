namespace DTOs
{
    public class ProductDTO
    {
        public required string ProductID { get; set; }
        public required string ProductName { get; set; }
        public required string BrandName { get; set; }
        public required string ProductCategory  { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
    }
}