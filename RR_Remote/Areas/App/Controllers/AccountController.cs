using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RR_Remote.Context;
using RR_Remote.IUtilities;
using RR_Remote.Models.ApiDTO;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;
using RR_Remote.Utilities;

namespace RR_Remote.Areas.App.Controllers
{
    [Area("App")]
    [Route("api/{controller}/{action}")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAccount _account;
        public AccountController(AppDbContext context, IAccount account)
        {
            _context = context;
            _account = account;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO accountModel)
        {
            try
            {
                var response = new Response<User>();

                bool Check = _context.Users.Any(x => x.MobileNumber == accountModel.UserName && x.Password == accountModel.Password);
                if (Check)
                {
                    var data = await _context.Users
                       .Where(x => x.MobileNumber == accountModel.UserName && x.Password == accountModel.Password)
                       .Select(x => new
                       {
                           x.Id,
                           x.Name,
                           x.Email,
                           x.MobileNumber,
                           x.CreatedDate
                       })
                       .FirstOrDefaultAsync();
                    int UserID = data.Id;

                    response.Succeeded = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = "Success";
                    response.Message = "Login Successfully Here.";
                    return Ok(new { response, data });
                }
                return BadRequest(new { Message = "Invalid user id or password" });
            }
            catch (Exception ex)
            {

                throw new Exception("Error Message :" + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Signup(User model)
        {
            try
            {
                var response = new Response<User>();
                var data = _context.Users.Where(x => x.MobileNumber == model.MobileNumber).FirstOrDefault();
                if(data!=null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Mobile number already exist.";
                    return BadRequest(new { response });
                }
                bool Check = await _account.AddUser(model);
                if (Check)
                {
                    response.Succeeded = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = "Success";
                    response.Message = "User added successfully.";
                    return Ok(new { response });
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Failed to add the user.";
                    return BadRequest(new { response });
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error Message :" + ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                var response = new Response<User>();
                var data = await _account.GetProfileDetail(userId);
                if (data != null)
                {
                    return Ok(new { Succeeded=true, StatusCode=200, Status="Success", Message= "Profile detail retrieved successfully." ,data });                    
                }
                else
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "User not found.";
                    return NotFound(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            var response = new Response<ChangePassword>();

            try
            {
                var record = _context.Users.Find(model.Id);

                if (record == null)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Status = "Failed";
                    response.Message = "User not found.";
                    return BadRequest(response);
                }

                if (record.Password != model.OldPassword)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Old password is incorrect.";
                    return BadRequest(response);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "Confirm password does not match.";
                    return BadRequest(response);
                }

                if (record.Password == model.Password)
                {
                    response.Succeeded = false;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Status = "Failed";
                    response.Message = "New password cannot be the same as the old password.";
                    return BadRequest(response);
                }

                bool isChanged = await _account.ChangePass(model);
                if (isChanged)
                {
                    response.Succeeded = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Status = "Success";
                    response.Message = "Password has been updated successfully.";
                    return Ok(response);
                }

                response.Succeeded = false;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = "Failed";
                response.Message = "Failed to update the password. Please try again later.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = "Error";
                response.Message = $"An unexpected error occurred: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
