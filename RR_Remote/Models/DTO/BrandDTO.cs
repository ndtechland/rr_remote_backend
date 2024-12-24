using RR_Remote.Models.Entity;

namespace RR_Remote.Models.DTO
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public IFormFile ImageFile { get; set; }
        public IEnumerable<Brand> BrandList { get; set; }
    }
}
