using Dapper;
using RR_Remote.Context;
using RR_Remote.Models.Entity;

namespace RR_Remote.Repositry
{
    public class DbRepository:IDbRepository
    {
        private readonly DapperContext _context;
        public DbRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM Users";

            using (var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<User>(query);
                return companies.ToList();
            }
        }
        public async Task<IEnumerable<Brand>> GetBrands()
        {
            var query = "SELECT * FROM Brand";

            using (var connection = _context.CreateConnection())
            {
                var data = await connection.QueryAsync<Brand>(query);
                return data.ToList();
            }
        }
    }
}
