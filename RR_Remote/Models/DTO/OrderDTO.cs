namespace RR_Remote.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<OrderList> OrderLists {  get; set; } 
    }
    public class OrderHistory
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class OrderList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public double ProductPrice { get; set; }
        public double TotalPrice { get; set; }
        public string ProductImage { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
