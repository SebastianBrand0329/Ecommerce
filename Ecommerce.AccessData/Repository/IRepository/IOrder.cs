using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository.IRepository
{
    public interface IOrder : IRepository<Order>
    {
        void Update(Order order);

        void UpdateState(int id, string OrderState, string PayState);

        void UpdatePayStripe(int id, string sessionId, string transactionId);
    }

}
