using RR_Remote.Models.Entity;

namespace RR_Remote.Repositry
{
    public interface IDbRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<IEnumerable<Brand>> GetBrands();
    }
}
