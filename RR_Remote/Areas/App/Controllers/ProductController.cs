using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;
using RR_Remote.Utilities;

namespace RR_Remote.Areas.App.Controllers
{
    [Area("App")]
    [Route("api/{controller}/{action}")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }
        public async Task<IActionResult> Brands()
        {
            try
            {
                var response = new Response<Brand>();
                var data = await _product.GetBrands();
                if (data != null)
                {
                    return Ok(new { Succeeded = true, StatusCode = 200, Status = "Success", Message = "Brands retrieved successfully.", data });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Brands not available.";
                    return NotFound(response);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> CategoriesByBrandId(int BrandId)
        {
            try
            {
                var response = new Response<CategoryMaster>();
                var data = await _product.GetCategoryByBrandId(BrandId);
                if (data.Count()>0)
                {
                    return Ok(new { Succeeded = true, StatusCode = 200, Status = "Success", Message = "Categories retrieved successfully.", data });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Categories not available.";
                    return NotFound(response);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> ProductsByCatId(int CatId)
        {
            try
            {
                var response = new Response<Product>();
                var data = await _product.GetProductsByCatId(CatId);
                if (data.Count()>0)
                {
                    return Ok(new { Succeeded = true, StatusCode = 200, Status = "Success", Message = "Products retrieved successfully.", data });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Products not available.";
                    return NotFound(response);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> ProductById(int Id)
        {
            try
            {
                var response = new Response<Product>();
                var data = await _product.GetProductById(Id);
                if (data!=null)
                {
                    return Ok(new { Succeeded = true, StatusCode = 200, Status = "Success", Message = "Product Detail retrieved successfully.", data });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Product not available.";
                    return NotFound(response);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
