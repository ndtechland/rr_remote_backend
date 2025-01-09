using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                var response = new Response<CheckoutDTO>();
                var productInfo = _context.Products.Find(model.ProductId);

                if (productInfo == null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "The product you are trying to order is not available. Please try again..";
                    return NotFound(response);
                }

                if (model.Qty > 0)
                {
                    //bool isCreated = await _order.Order(model);
                    int orderId = await _order.Order(model);
                    if (orderId > 0)
                    {                         
                        var checkoutData = new CheckoutDTO
                        {       
                            OrderId=orderId,
                            TotalAmount = model.Qty * productInfo.ProductPrice,
                            ProductName = productInfo.ProductName,
                            ProductImage = productInfo.ProductImage,
                            ProductPrice = productInfo.ProductPrice,
                            Quantity = model.Qty,
                            CheckoutDate = DateTime.Now
                        };

                        response.Succeeded = true;
                        response.StatusCode = StatusCodes.Status200OK;
                        response.Status = "Success";
                        response.Message = "Your order has been successfully placed! Proceed to checkout to complete your purchase.";
                        response.Data = checkoutData;

                        return Ok(response);
                    }
                    else
                    {
                        response.Succeeded = false;
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Status = "Failed";
                        response.Message = "We encountered an issue while processing your order. Please try again later.";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Unable to proceed. Please add at least one item to your order.";
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }

        //public async Task<IActionResult> Order(OrderDTO model)
        //{
        //    try
        //    {
        //        var response = new Response<Order>();
        //        var productinfo=_context.Products.Find(model.ProductId);
        //        if (productinfo == null)
        //        {
        //            response.Succeeded = false;
        //            response.StatusCode = StatusCodes.Status404NotFound;
        //            response.Status = "Failed";
        //            response.Message = "Product not available.";
        //            return NotFound(response);
        //        }
        //        if(model.Qty>0)
        //        {
        //            bool isCreated = await _order.Order(model);
        //            if (isCreated)
        //            {
        //                response.Succeeded = true;
        //                response.StatusCode = StatusCodes.Status200OK;
        //                response.Status = "Success";
        //                response.Message = "Products ordered successfully.";
        //                return Ok(response);
        //            }
        //            else
        //            {
        //                response.Succeeded = false;
        //                response.StatusCode = StatusCodes.Status400BadRequest;
        //                response.Status = "Failed";
        //                response.Message = "Failed.";
        //                return BadRequest(response);
        //            }
        //        }
        //        else
        //        {
        //            response.Succeeded = false;
        //            response.StatusCode = StatusCodes.Status400BadRequest;
        //            response.Status = "Failed";
        //            response.Message = "Please add at least one item.";
        //            return BadRequest(response);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
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
        
        public async Task<IActionResult> GetCheckoutDetails(int userId)
        {
            try
            {
                var response = new Response<CheckoutDTO>();

                // Validate OrderId
                if (userId <= 0)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Invalid User ID.";
                    return BadRequest(response);
                }

                var orderinfo = _context.Orders.Where(o=>o.UserId== userId && o.IsOrder==false).OrderByDescending(o=>o.Id).FirstOrDefault();

                if (orderinfo == null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Order not found.";
                    return NotFound(response);
                }
                
                var productInfo = _context.Products.Find(orderinfo.ProductId);
                
                var checkoutDetails = new CheckoutDTO
                {
                    OrderId = orderinfo.Id,
                    ProductName = productInfo.ProductName,
                    ProductImage = productInfo.ProductImage,
                    ProductPrice = productInfo.ProductPrice,
                    Quantity = orderinfo.Qty,
                    CheckoutDate = orderinfo.CheckoutDate,
                    TotalAmount = productInfo.ProductPrice * orderinfo.Qty
                };

                response.Succeeded = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Status = "Success";
                response.Message = "Checkout details retrieved successfully.";
                response.Data = checkoutDetails;

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving checkout details.",
                    Details = ex.Message
                });
            }
        }

        public async Task<IActionResult> OrderPlace(PlaceOrder model)
        {
            try
            {
                var response = new Response<PlaceOrder>();
                if (model==null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Missing required field.";
                    return BadRequest(response);
                }
                var orderinfo = _context.Orders.Find(model.OrderId);
                if(orderinfo==null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "Order detail not found.";
                    return NotFound(response);
                }

                bool isOrdered = await _order.OrderPlace(model);
                if (isOrdered)
                {
                    response.Succeeded = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = "Success";
                    response.Message = "Order placed successfully.";
                    return Ok(response);
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Failed to place the order. Please try again.";
                    return BadRequest(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
