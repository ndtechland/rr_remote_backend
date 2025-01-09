using RR_Remote.Models.ApiDTO;

namespace RR_Remote.Services.ContractApi
{
    public interface IOrder
    {
        Task<int> Order(OrderDTO model);
        Task<List<OrderHistory>> GetOrders(int UserId);
        Task<bool> OrderPlace(PlaceOrder model);
    }
}
