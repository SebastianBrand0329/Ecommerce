using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IOrder : IRepository<Order>
    {
        void Update(Order order);
    }
}
