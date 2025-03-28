namespace DTOs
{
    public class OrderProduct
    {
        public required string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderLine
    {
        public required string Customer { get; set; }
        public string Email { get; set; }
        public required List<OrderProduct> Products { get; set; }
        public required int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? TrackAndTrace { get; set; }
    }
}