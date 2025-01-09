using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using RR_Remote.Context;
using RR_Remote.IUtilities;
using RR_Remote.Models.ApiDTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.Contract;
using RR_Remote.Services.ContractApi;
using RR_Remote.Utilities;

namespace RR_Remote.Areas.App.Controllers
{
    [Area("App")]
    [Route("api/{controller}/{action}")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _order;
        private readonly AppDbContext _context;
        public OrderController(IOrder order, AppDbContext context)
        {
            _order = order;
            _context = context;
        }
        public async Task<IActionResult> Order(OrderDTO model)
        {
            try
            {
                var response = new Response<Order>();
                var productinfo=_context.Products.Find(model.ProductId);
                if (productinfo == null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Product not available.";
                    return NotFound(response);
                }
                if(model.Qty>0)
                {
                    bool isCreated = await _order.Order(model);
                    if (isCreated)
                    {
                        response.Succeeded = true;
                        response.StatusCode = StatusCodes.Status200OK;
                        response.Status = "Success";
                        response.Message = "Products ordered successfully.";
                        return Ok(response);
                    }
                    else
                    {
                        response.Succeeded = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Status = "Failed";
                        response.Message = "Failed.";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Please add at least one item.";
                    return BadRequest(response);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> OrderHistory(int UserId)
        {
            try
            {
                var response=new Response<OrderHistory>();
                var data = await _order.GetOrders(UserId);
                if(data.Count > 0)
                {
                    return Ok(new { Succeeded = true, StatusCode = 200, Status = "Success", Message = "Order history retrieved successfully.", data });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "No order history found. You haven't placed any orders yet.";
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
