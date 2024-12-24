using RR_Remote.Context;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;

namespace RR_Remote.Services.ImplementationApi
{
    public class CommonImplementation:ICommon
    {
		private readonly AppDbContext _context;
        public CommonImplementation(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BannerMaster>> GetBanner()
        {
			try
			{
				var data=_context.BannerMasters.Where(x=>x.IsActive).ToList();
                return data;
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
