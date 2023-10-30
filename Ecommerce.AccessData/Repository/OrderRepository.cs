using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

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
    }
}
