using Microsoft.AspNetCore.Identity;
using RR_Remote.Context;
using RR_Remote.Models.ApiDTO;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;

namespace RR_Remote.Services.ImplementationApi
{
    public class AccountImplementation:IAccount
    {
        private readonly AppDbContext _context;
        public AccountImplementation(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Login(User model)
        {
			try
			{
				if(model.MobileNumber!=null && model.Password!=null)
				{
					var check=_context.Users.Where(x=>x.MobileNumber==model.MobileNumber && x.Password==model.Password).FirstOrDefault();
                    if (check != null)
                    {
                        return true;
                    }
                    else 
                    { 
                        return false; 
                    }  
				}
                return false;
			}
			catch (Exception)
			{

				throw;
			}
        }
        public async Task<bool> AddUser(User model)
        {
            try
            {
                if(model!=null)
                {
                    var data = new User()
                    {
                        Name = model.Name,
                        MobileNumber = model.MobileNumber,
                        Email = model.Email,
                        Password = model.Password,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    };
                    _context.Add(data);
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
        public async Task<User> GetProfileDetail(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);

                if (user == null)
                {
                    return null;
                }

                User data = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    MobileNumber = user.MobileNumber,
                    Email = user.Email,
                    CreatedDate = user.CreatedDate,
                };

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> ChangePass(ChangePassword model)
        {
            try
            {
                var data = _context.Users.Find(model.Id);

                data.Password = model.Password;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
