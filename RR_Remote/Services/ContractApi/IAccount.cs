using RR_Remote.Models.ApiDTO;
using RR_Remote.Models.DTO;
using RR_Remote.Models.Entity;

namespace RR_Remote.Services.ContractApi
{
    public interface IAccount
    {
        Task<bool> AddUser(User model);
        Task<bool> ChangePass(ChangePassword model);
        Task<User> GetProfileDetail(int userId);
    }
}
