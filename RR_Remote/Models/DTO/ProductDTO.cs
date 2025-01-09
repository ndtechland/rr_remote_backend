using Microsoft.AspNetCore.Mvc.Rendering;

namespace RR_Remote.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public double ProductPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public IFormFile ImageFile { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Categories { get; set; }
        public IEnumerable<ProductDetails> Products { get; set; }

    }
    public class ProductDetails
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
