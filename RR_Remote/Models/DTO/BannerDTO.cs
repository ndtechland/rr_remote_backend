using RR_Remote.Models.Entity;

namespace RR_Remote.Models.DTO
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BannerImage { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<BannerMaster> BannerList { get; set; }
    }
}
