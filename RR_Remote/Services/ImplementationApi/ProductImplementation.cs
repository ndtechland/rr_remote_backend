using RR_Remote.Context;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;

namespace RR_Remote.Services.ImplementationApi
{
    public class ProductImplementation:IProduct
    {
		private readonly AppDbContext _context;
        public ProductImplementation(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Brand>> GetBrands()
        {
            try
            {
                var data = _context.Brands.Where(x => x.IsActive).ToList();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
			}
			catch (Exception)
			{

				throw;
			}
        }
        public async Task<List<CategoryMaster>> GetCategoryByBrandId(int BrandId)
        {
            try
            {
                var data = _context.CategoryMasters.Where(x => x.IsActive && x.BrandId== BrandId).ToList();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Product>> GetProductsByCatId(int CatId)
        {
            try
            {
                var data = _context.Products.Where(x => x.IsActive && x.CategoryId == CatId).ToList();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Product> GetProductById(int Id)
        {
            try
            {
                var data = _context.Products.Where(x => x.IsActive && x.Id == Id).FirstOrDefault();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
