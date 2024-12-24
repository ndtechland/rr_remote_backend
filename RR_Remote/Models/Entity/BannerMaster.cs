namespace RR_Remote.Models.Entity
{
    public class BannerMaster
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BannerImage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
