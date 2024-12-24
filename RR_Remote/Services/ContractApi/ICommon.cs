using RR_Remote.Models.Entity;

namespace RR_Remote.Services.ContractApi
{
    public interface ICommon
    {
        Task<List<BannerMaster>> GetBanner();
    }
}
