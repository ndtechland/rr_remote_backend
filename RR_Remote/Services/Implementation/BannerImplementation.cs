using Microsoft.EntityFrameworkCore;
using RR_Remote.Context;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.Contract;

namespace RR_Remote.Services.Implementation
{

    public class BannerImplementation:IBanner
    {
        private readonly AppDbContext _context;
        public BannerImplementation(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUpdateBanner(BannerDTO model)
        {
            try
            {
                if (model != null)
                {
                    if (model.Id == 0)
                    {
                        var data = new BannerMaster()
                        {
                            Title = model.Title,
                            BannerImage = model.BannerImage,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(data);

                    }
                    else
                    {
                        var data = _context.BannerMasters.Find(model.Id);
                        data.Title = model.Title;
                        if (model.BannerImage != null)
                        {
                            data.BannerImage = model.BannerImage;
                        }
                    }
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<BannerMaster>> GetBanner()
        {
            try
            {
                var data = _context.BannerMasters.Where(x => x.IsActive).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteBanner(int id)
        {
            try
            {
                var data = _context.BannerMasters.Find(id);
                if(data != null)
                {
                    data.IsActive = false;
                    _context.SaveChanges();
                    return true;
                }
                return false;
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
