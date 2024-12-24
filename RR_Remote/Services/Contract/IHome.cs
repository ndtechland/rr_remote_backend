using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;

namespace RR_Remote.Services.Contract
{
    public interface IHome
    {
        Task<bool> AddUpdateBrand(BrandDTO model);
        Task<List<Brand>> GetBrands();
        Task<bool> DeleteBrand(int id);
        Task<bool> AddUpdateProduct(ProductDTO model);
        Task<bool> DeleteProduct(int id);
        Task<List<ProductDetails>> GetProducts();
        Task<bool> AddUpdateCategory(CategoryDTO model);
        Task<bool> DeleteCategory(int id);
        Task<List<CategoryDetails>> GetCategories();
        Task<bool> Login(LoginDTO login);
        Task<bool> ChangePassword(ChangePasswordDTO model);
        Task<List<User>> GetUsers();
    }
}
