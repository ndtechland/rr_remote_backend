using Microsoft.AspNetCore.Mvc.Rendering;
using RR_Remote.Models.Entity;

namespace RR_Remote.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public SelectList Brands { get; set; }
        public IEnumerable<CategoryDetails> CategoryList { get; set; }
    }
    public class CategoryDetails
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
