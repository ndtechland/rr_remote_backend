namespace RR_Remote.Models.ApiDTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class OrderHistory
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double TotalPrice { get; set; }
        public string ProductImage { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public int Qty { get; set; }
        public DateTime OrderDate { get; set; }
    }
    
}