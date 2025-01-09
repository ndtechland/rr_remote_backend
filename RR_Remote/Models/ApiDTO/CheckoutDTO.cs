namespace RR_Remote.Models.ApiDTO
{
    public class CheckoutDTO
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public double ProductPrice { get; set; }
        public double TotalAmount { get; set; }
        public DateTime CheckoutDate { get; set; }
    }
    public class PlaceOrder
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
    }
}
