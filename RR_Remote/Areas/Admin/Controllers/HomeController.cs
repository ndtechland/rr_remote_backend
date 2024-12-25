using ComplianceKare.Admin.Extentions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RR_Remote.Areas.Admin.IUtilities;
using RR_Remote.Context;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Repositry;
using RR_Remote.Services.Contract;
using System.Security.Claims;

namespace RR_Remote.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHome _home;
        private readonly IDbRepository _db;
        private readonly IImageUpload _imageUpload;
        public HomeController(AppDbContext context, IHome home, IDbRepository db, IImageUpload imageUpload)
        {
            _context = context;
            _home = home;
            _db = db;
            _imageUpload = imageUpload;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsActive = await _home.Login(model);
                    if (IsActive == true)
                    {
                        AdminLogin login = await _context.AdminLogins
                            .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                            .FirstOrDefaultAsync();

                        if (login != null)
                        {
                            HttpContext.Session.Set<AdminLogin>("userkey", login);

                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.UserName.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                        new Claim(ClaimTypes.PrimarySid, login.Id.ToString())
                    };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = false
                            };

                            await HttpContext.SignInAsync("Identity.Application", claimsPrincipal, authProperties);

                            TempData["SuccessMessage"] = "Login successful!";
                            return RedirectToAction("Dashboard");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid username or password.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid username or password.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please enter valid login details.";
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {

                throw new Exception("Error Message :" + ex.Message);
            }
        }
        public IActionResult ChangePassword()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                if (model != null)
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        TempData["errormsg"] = "Passwords do not match.";
                        return RedirectToAction("ChangePassword");
                    }
                    var userid = User.Identity?.Name?.ToString();
                    model.UserName = userid;
                    var user = await _home.ChangePassword(model);
                    TempData["msg"] = "Password changed successfully. Please login with your new password.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["errormsg"] = "Username or password not valid.";
                    return RedirectToAction("ChangePassword");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Message :" + ex.Message);
            }
        }
        public IActionResult Dashboard()
        {
            try
            {
                ViewBag.UserCount = _context.Users.Where(x=>x.IsActive).Count();
                ViewBag.BrandCount = _context.Brands.Where(x=>x.IsActive).Count();
                ViewBag.CategoryCount = _context.CategoryMasters.Where(x=>x.IsActive).Count();
                ViewBag.ProductCount = _context.Products.Where(x=>x.IsActive).Count();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Brands(int id)
        {
            try
            {
                var model = new BrandDTO();
                model.BrandList = await _home.GetBrands();
                int iId = (int)(id == null ? 0 : id);
                ViewBag.Id = 0;
                ViewBag.BrandName = "";
                ViewBag.Image = "";
                ViewBag.heading = "Add Brand";
                ViewBag.btnText = "SAVE";
                if (iId != null && iId != 0)
                {
                    var data = _context.Brands.Find(iId);
                    if (data != null)
                    {
                        ViewBag.id = data.Id;
                        ViewBag.BrandName = data.BrandName;
                        ViewBag.Image = data.Image;
                        ViewBag.btnText = "UPDATE";
                        ViewBag.Heading = "Update Brand";

                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Brands(BrandDTO model, IFormCollection model1)
        {
            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageFile = model1.Files[0];

                    var uploadResult = _imageUpload.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["errormsg"] = "Only .jpg, .jpeg, .png, and .gif files are allowed.";
                        return RedirectToAction("Brands");
                    }

                    model.Image = uploadResult;
                }
                bool isCreated = await _home.AddUpdateBrand(model);
                if (isCreated)
                {
                    TempData["msg"] = model.Id > 0
                        ? "Record has been updated successfully."
                        : "Record has been added successfully.";
                    return RedirectToAction("Brands");
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }
        public async Task<IActionResult> BrandDelete(int id)
        {
            try
            {
                bool IsDelete = await _home.DeleteBrand(id);
                if(IsDelete)
                {
                    TempData["msg"] = "Deleted successfully.";                    
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                }
                return RedirectToAction("Brands");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public IActionResult Products(int id=0)
        {
            try
            {
                var model = new ProductDTO();
                model.Brands = new SelectList(_context.Brands.Where(a=>a.IsActive).ToList(), "Id", "BrandName");
                model.Categories = new SelectList(_context.CategoryMasters.Where(a=>a.IsActive).ToList(), "Id", "CategoryName");
                if (id > 0)
                {
                    var existdata = _context.Products.Where(x => x.Id == id).FirstOrDefault();
                    model.Id = existdata.Id;
                    model.ProductName = existdata.ProductName;
                    model.ProductImage = existdata.ProductImage;
                    model.Description = existdata.Description;
                    model.ProductImage = existdata.ProductImage;
                    model.CategoryId = existdata.CategoryId;
                    model.BrandId = existdata.BrandId;
                    ViewBag.Heading = "Update Product";
                    ViewBag.BtnTXT = "Update";
                    return View(model);
                }
                else
                {
                    model.Id = 0;
                    model.CategoryId     = 0;
                    model.BrandId = 0;
                    model.ProductImage = "";
                    model.ProductName = "";
                    model.Description = "";
                    ViewBag.BtnTXT = "Save";
                    ViewBag.Heading = "Add Product";
                    return View(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Products(ProductDTO model, IFormCollection model1)
        {
            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageFile = model1.Files[0];

                    var uploadResult = _imageUpload.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["errormsg"] = "Only .jpg, .jpeg, .png, and .gif files are allowed.";
                        return RedirectToAction("Products");
                    }

                    model.ProductImage = uploadResult;
                }
                bool isCreated = await _home.AddUpdateProduct(model);
                if (isCreated)
                {
                    TempData["msg"] = model.Id > 0
                        ? "Record has been updated successfully."
                        : "Record has been added successfully.";
                    return RedirectToAction("Products");
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }
        public async Task<IActionResult> ProductList()
        {
            try
            {
                var model = new ProductDTO();
                model.Products = await _home.GetProducts();
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> ProductDelete(int id)
        {
            try
            {
                bool IsDelete = await _home.DeleteProduct(id);
                if (IsDelete)
                {
                    TempData["msg"] = "Deleted successfully.";
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                }
                return RedirectToAction("ProductList");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Category(int id)
        {
            try
            {
                var model = new CategoryDTO();
                model.Brands = new SelectList(_context.Brands.Where(a => a.IsActive).ToList(), "Id", "BrandName");

                model.CategoryList = await _home.GetCategories();
                int iId = (int)(id == null ? 0 : id);
                ViewBag.Id = 0;
                model.CategoryName = "";
                model.Image = "";
                model.BrandId = 0;
                ViewBag.heading = "Add Category";
                ViewBag.btnText = "SAVE";
                if (iId != null && iId != 0)
                {
                    var data = _context.CategoryMasters.Find(iId);
                    if (data != null)
                    {
                        ViewBag.id = data.Id;
                        model.CategoryName = data.CategoryName;
                        model.Image = data.Image;
                        model.BrandId = data.BrandId;
                        ViewBag.btnText = "UPDATE";
                        ViewBag.Heading = "Update Category";

                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Category(CategoryDTO model, IFormCollection model1)
        {
            try
            {
                if (model.ImageFile != null)
                {
                    model.ImageFile = model1.Files[0];

                    var uploadResult = _imageUpload.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["errormsg"] = "Only .jpg, .jpeg, .png, and .gif files are allowed.";
                        return RedirectToAction("Category");
                    }

                    model.Image = uploadResult;
                }
                bool isCreated = await _home.AddUpdateCategory(model);
                if (isCreated)
                {
                    TempData["msg"] = model.Id > 0
                        ? "Record has been updated successfully."
                        : "Record has been added successfully.";
                    return RedirectToAction("Category");
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }
        public async Task<IActionResult> CategoryDelete(int id)
        {
            try
            {
                bool IsDelete = await _home.DeleteCategory(id);
                if (IsDelete)
                {
                    TempData["msg"] = "Deleted successfully.";
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                }
                return RedirectToAction("Category");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> UsersList()
        {
            try
            {
                var data = await _home.GetUsers();
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
