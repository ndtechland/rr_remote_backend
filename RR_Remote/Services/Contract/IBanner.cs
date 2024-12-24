using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;

namespace RR_Remote.Services.Contract
{
    public interface IBanner
    {
        Task<bool> AddUpdateBanner(BannerDTO model);
        Task<List<BannerMaster>> GetBanner();
        Task<bool> DeleteBanner(int id);
    }
}
