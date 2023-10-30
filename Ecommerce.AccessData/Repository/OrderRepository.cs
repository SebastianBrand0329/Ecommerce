using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.AccessData.Repository
{
    public class OrderRepository : Repository<Order>, IOrder
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Order order)
        {
            _context.Update(order);
        }

        public void UpdatePayStripe(int id, string sessionId, string transactionId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order != null) 
            {
                if (!String.IsNullOrEmpty(sessionId))
                {
                    order.SessionId = sessionId;
                }

                if (!String.IsNullOrEmpty(transactionId))
                {
                    order.TransactionId = transactionId;
                    order.PayDate = DateTime.Now;   
                }
            }
        }

        public void UpdateState(int id, string OrderState, string PayState)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order != null)
            {
                order.Stateorder = OrderState;
                order.statePay = PayState;

            }
        }
    }
}
