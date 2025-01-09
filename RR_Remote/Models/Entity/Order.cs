namespace RR_Remote.Models.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
