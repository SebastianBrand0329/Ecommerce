using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IOrderDetail : IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
