using Microsoft.AspNetCore.Mvc;
using RR_Remote.Areas.Admin.IUtilities;
using RR_Remote.Context;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.Contract;

namespace RR_Remote.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IImageUpload _imageUpload;
        private readonly IBanner _banner;
        public BannerController(AppDbContext context, IImageUpload imageUpload, IBanner banner)
        {
            _context = context;
            _imageUpload = imageUpload;
            _banner = banner;
        }
        public async Task<IActionResult> Banner(int id=0)
        {
            try
            {
                var model = new BannerDTO();
                model.BannerList = await _banner.GetBanner();
                if (id > 0)
                {
                    var existdata = _context.BannerMasters.Where(x => x.Id == id).FirstOrDefault();
                    model.Id = existdata.Id;
                    model.BannerImage = existdata.BannerImage;
                    model.Title = existdata.Title;
                    ViewBag.Heading = "Update Banner";
                    ViewBag.BtnTXT = "Update";
                    return View(model);
                }
                else
                {
                    model.Id = 0;
                    model.BannerImage = "";
                    model.Title = "";
                    ViewBag.BtnTXT = "Save";
                    ViewBag.Heading = "Add Banner";
                    return View(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Banner(BannerDTO model, IFormCollection model1)
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
                        return RedirectToAction("Banner");
                    }

                    model.BannerImage = uploadResult;
                }
                bool isCreated = await _banner.AddUpdateBanner(model);
                if (isCreated)
                {
                    TempData["msg"] = model.Id > 0
                        ? "Banner has been updated successfully."
                        : "Banner has been added successfully.";
                    return RedirectToAction("Banner");
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
        public async Task<IActionResult> BannerDelete(int id)
        {
            try
            {
                bool IsDelete = await _banner.DeleteBanner(id);
                if (IsDelete)
                {
                    TempData["msg"] = "Deleted successfully.";
                }
                else
                {
                    TempData["errormsg"] = "Failed.";
                }
                return RedirectToAction("Banner");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
