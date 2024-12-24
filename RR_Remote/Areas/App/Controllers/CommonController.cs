using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;
using RR_Remote.Utilities;

namespace RR_Remote.Areas.App.Controllers
{
    [Area("App")]
    [Route("api/{controller}/{action}")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommon _common;
        public CommonController(ICommon common)
        {
            _common= common;
        }
        public async Task<IActionResult> GetBanners()
        {
            try
            {
                var response = new Response<BannerMaster>();
                var data = await _common.GetBanner();
                if (data != null)
                {
                    return Ok(new { Succeeded = true, StatusCode = 200, Status = "Success", Message = "Banners retrieved successfully.", data });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Banner not available.";
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
