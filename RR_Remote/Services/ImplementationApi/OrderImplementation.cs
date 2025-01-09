using RR_Remote.Context;
using RR_Remote.Models.ApiDTO;
using RR_Remote.Models.Entity;
using RR_Remote.Services.ContractApi;

namespace RR_Remote.Services.ImplementationApi
{
    public class OrderImplementation:IOrder
    {
        private readonly AppDbContext _context;
        public OrderImplementation(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Order(OrderDTO model)
        {
            try
            {
                var data = new Order()
                {
                    UserId = model.UserId,
                    ProductId = model.ProductId,
                    Qty = model.Qty,
                    OrderDate = DateTime.Now
                };
                 _context.Add(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<OrderHistory>> GetOrders(int UserId)
        {
            try
            {
                var data = (from o in _context.Orders
                            join p in _context.Products on o.ProductId equals p.Id
                            join b in _context.Brands on p.BrandId equals b.Id
                            join c in _context.CategoryMasters on p.CategoryId equals c.Id
                            where o.UserId== UserId
                            orderby o.Id descending
                            select new OrderHistory
                            {
                                Id = o.Id,
                                ProductName = p.ProductName,
                                ProductImage = p.ProductImage,
                                ProductPrice = p.ProductPrice,
                                Qty = o.Qty,
                                TotalPrice = p.ProductPrice * o.Qty,
                                Brand = b.BrandName,
                                Category = c.CategoryName,
                                OrderDate = o.OrderDate
                            }
                          ).ToList();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
