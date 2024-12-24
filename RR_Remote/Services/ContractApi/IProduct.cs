using RR_Remote.Models.Entity;

namespace RR_Remote.Services.ContractApi
{
    public interface IProduct
    {
        Task<List<Brand>> GetBrands();
        Task<List<CategoryMaster>> GetCategoryByBrandId(int BrandId);
        Task<List<Product>> GetProductsByCatId(int CatId);
        Task<Product> GetProductById(int Id);
    }
}
