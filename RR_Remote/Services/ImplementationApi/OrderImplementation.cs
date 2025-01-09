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
        public async Task<int> Order(OrderDTO model)
        {
            try
            {
                var data = new Order()
                {
                    UserId = model.UserId,
                    ProductId = model.ProductId,
                    Qty = model.Qty,
                    IsOrder = false,
                    CheckoutDate = DateTime.Now
                };
                 _context.Add(data);
                _context.SaveChanges();
                return data.Id;
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
                            where o.UserId== UserId && o.IsOrder==true
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
        public async Task<bool> OrderPlace(PlaceOrder model)
        {
            try
            {
                var orderinfo = _context.Orders.Find(model.OrderId);
                var dd = _context.Orders.Where(o=>o.UserId==model.UserId && o.IsOrder==false).FirstOrDefault();
                
                if (orderinfo != null)
                {
                    orderinfo.IsOrder = true;
                    orderinfo.OrderDate = DateTime.Now;
                    _context.SaveChanges();
                   
                    if (dd.UserId == model.UserId && dd.IsOrder==false)
                    {                        
                        _context.Orders.Remove(dd);  
                        _context.SaveChanges(); 
                    }
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
    }
}
