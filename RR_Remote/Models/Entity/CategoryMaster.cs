namespace RR_Remote.Models.Entity
{
    public class CategoryMaster
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
