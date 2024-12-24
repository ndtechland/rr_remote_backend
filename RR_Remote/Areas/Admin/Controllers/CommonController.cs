using Microsoft.AspNetCore.Mvc;
using RR_Remote.Context;
namespace RR_Remote.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        private readonly AppDbContext _context;
        public CommonController(AppDbContext context)
        {
            _context = context;
        }
        public ActionResult GetCategoryByBrand(int brandId)
        {
            var data = _context.CategoryMasters.Where(a => a.BrandId == brandId).ToList();
            return Json(data);
        }
    }
}
