using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RR_Remote.Context;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.Contract;
using System.Security.Claims;

namespace RR_Remote.Services.Implementation
{
    public class HomeImplementation : IHome
    {
        private readonly AppDbContext _context;
        public HomeImplementation(AppDbContext context)
        {
            _context = context;
        }

        //brand is category and category is brand
        public async Task<bool> AddUpdateBrand(BrandDTO model)
        {
            try
            {
                if (model != null)
                {
                    if(model.Id==0)
                    {
                        var data = new Brand()
                        {
                            BrandName = model.BrandName,
                            Image = model.Image,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(data);
                        
                    }
                    else
                    {
                        var data = _context.Brands.Find(model.Id);
                        data.BrandName = model.BrandName;
                        if (model.Image != null)
                        {
                            data.Image = model.Image;
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
        public async Task<List<Brand>> GetBrands()
        {
            try
            {
                var data = _context.Brands.Where(x => x.IsActive).OrderByDescending(x=>x.Id).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteBrand(int id)
        {
            try
            {
                var data = _context.CategoryMasters.Find(id);
                data.IsActive = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> AddUpdateProduct(ProductDTO model)
        {
            try
            {
                if (model != null)
                {
                    if (model.Id == 0)
                    {
                        var data = new Product()
                        {
                            ProductName = model.ProductName,
                            ProductPrice = model.ProductPrice,
                            Description = model.Description,
                            ProductImage = model.ProductImage,
                            CategoryId = model.CategoryId,
                            BrandId = model.BrandId,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(data);

                    }
                    else
                    {
                        var data = _context.Products.Find(model.Id);
                        data.ProductName = model.ProductName;
                        data.ProductPrice = model.ProductPrice;
                        if (model.ProductImage != null)
                        {
                            data.ProductImage = model.ProductImage;
                        }
                        data.Description = model.Description;
                        data.CategoryId = model.CategoryId;
                        data.BrandId = model.BrandId;
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
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var data = _context.Products.Find(id);
                data.IsActive = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<ProductDetails>> GetProducts()
        {
            try
            {
                var data = (from p in _context.Products
                            join b in _context.Brands on p.BrandId equals b.Id
                            join c in _context.CategoryMasters on p.CategoryId equals c.Id
                            where p.IsActive
                            orderby p.Id descending
                            select new ProductDetails
                            {
                                Id = p.Id,
                                ProductImage = p.ProductImage,
                                ProductName = p.ProductName,
                                ProductPrice = p.ProductPrice,
                                IsActive = p.IsActive,
                                CreatedDate = p.CreatedDate,
                                BrandName = b.BrandName,
                                CategoryName = c.CategoryName

                            }
                          ).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> AddUpdateCategory(CategoryDTO model)
        {
            try
            {
                if (model != null)
                {
                    if (model.Id == 0)
                    {
                        var data = new CategoryMaster()
                        {
                            CategoryName = model.CategoryName,
                            Image = model.Image,
                            BrandId = model.BrandId,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };
                        _context.Add(data);

                    }
                    else
                    {
                        var data = _context.CategoryMasters.Find(model.Id);
                        data.CategoryName = model.CategoryName;
                        if (model.Image != null)
                        {
                            data.Image = model.Image;
                        }
                        data.BrandId = model.BrandId;
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
        public async Task<List<CategoryDetails>> GetCategories()
        {
            try
            {
                //var data = _context.CategoryMasters.Where(x => x.IsActive).ToList();
                var data = (from c in _context.CategoryMasters
                            join b in _context.Brands on c.BrandId equals b.Id
                            where c.IsActive
                            orderby c.Id descending
                            select new CategoryDetails
                            {
                                Id = c.Id,
                                CategoryName = c.CategoryName,
                                Image = c.Image,
                                CreatedDate = c.CreatedDate,
                                BrandName = b.BrandName
                            }
                          ).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var data = _context.Brands.Find(id);
                data.IsActive = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Login(LoginDTO login)
        {
            try
            {
                bool Ckeck = await _context.AdminLogins.AnyAsync(x => x.UserName == login.UserName && x.Password == login.Password);
                return Ckeck;
            }
            catch (Exception ex)
            {

                throw new Exception("Error Message :" + ex.Message);
            }
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                var result = await _context.AdminLogins.FirstOrDefaultAsync(x => x.UserName == model.UserName);
                if (result != null)
                {
                    result.Password = model.Password;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error :" + ex.Message);
            }
        }
        public async Task<List<User>> GetUsers()
        {
            try
            {
                var data = _context.Users.Where(x=>x.IsActive).OrderByDescending(x=>x.Id).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<OrderList>> GetOrders()
        {
            try
            {
                var data = (from o in _context.Orders
                            join u in _context.Users on o.UserId equals u.Id
                            join p in _context.Products on o.ProductId equals p.Id
                            join b in _context.Brands on p.BrandId equals b.Id
                            join c in _context.CategoryMasters on p.CategoryId equals c.Id
                            orderby o.Id descending
                            select new OrderList
                            {
                                Id = o.Id,
                                UserName = u.Name,
                                Mobile = u.MobileNumber,
                                Email = u.Email,
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                ProductPrice = p.ProductPrice,
                                Qty = o.Qty,
                                TotalPrice = p.ProductPrice * o.Qty,
                                Brand = b.BrandName,
                                Category = c.CategoryName,
                                OrderDate = o.OrderDate
                            }
                          ).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
