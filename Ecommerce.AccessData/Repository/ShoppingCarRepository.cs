using Ecommerce.AccessData.Data;
using Ecommerce.AccessData.Repository.IRepository;
using Ecommerce.Models;

namespace Ecommerce.AccessData.Repository
{
    public class ShoppingCarRepository : Repository<ShoppingCar>, IShoppinCar
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCarRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShoppingCar shoppingCar)
        {
            _context.Update(shoppingCar);
        }
    }
}
